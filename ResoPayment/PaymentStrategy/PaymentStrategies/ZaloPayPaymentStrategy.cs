using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;
using ResoPayment.Constants;
using ResoPayment.Enums;
using ResoPayment.Helpers;
using ResoPayment.Infrastructure.PaymentConfigModels;
using ZaloPay.Helper.Crypto;
using ZaloPay.Helper;
using ResoPayment.Payload.Response;

namespace ResoPayment.PaymentStrategy.PaymentStrategies
{
    public class ZaloPayPaymentStrategy : IPaymentStrategy
    {
        private readonly ZaloPayConfig _zaloPayConfig;
        private readonly Guid _orderId;
        private readonly string _orderDescription;
        private readonly double _amount;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ZaloPayPaymentStrategy(string zaloPayConfigJson, Guid orderId, string orderDescription, double amount, IHttpContextAccessor httpContextAccessor)
        {
            _orderDescription = orderDescription;
            _amount = amount;
            _httpContextAccessor = httpContextAccessor;
            _orderId = orderId;
            _zaloPayConfig = JsonConvert.DeserializeObject<ZaloPayConfig>(zaloPayConfigJson) ?? throw new InvalidOperationException();
        }

        public async Task<CreatePaymentResponse> ExecutePayment()
        {
            var embeddata = new { merchantinfo = "DeerCoffee", redirecturl = "https://" + _httpContextAccessor.HttpContext.Request.Host.Value + ApiEndPointConstant.Payment.ZaloPayEndpoint };
            var items = new[]{
                new { itemid = "it1", itemname = "Thanh toan don hang", itemprice = _amount, itemquantity = 1 } };

            var param = new Dictionary<string, string>();
            param.Add("appid", _zaloPayConfig.AppId);
            param.Add("appuser", "User");
            param.Add("apptime", Utils.GetTimeStamp().ToString());
            param.Add("amount", _amount.ToString());
            param.Add("apptransid", DateTimeHelper.ConvertDateTimeToVietNamTimeZone().ToString("yyMMdd") + "_" + _orderId); // mã giao dich có định dạng yyMMdd_xxxx
            param.Add("embeddata", JsonConvert.SerializeObject(embeddata));
            param.Add("item", JsonConvert.SerializeObject(items));
            param.Add("description", _orderDescription);
            param.Add("bankcode", _zaloPayConfig.BankCode);
            //param.Add("callbackurl", "https://" + _httpContextAccessor.HttpContext.Request.Host.Value + ApiEndPointConstant.Payment.ZaloPayEndpoint);

            var data = _zaloPayConfig.AppId + "|" + param["apptransid"] + "|" + param["appuser"] + "|" + param["amount"] + "|"
                + param["apptime"] + "|" + param["embeddata"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _zaloPayConfig.Key1, data));

            var result = await HttpHelper.PostFormAsync(_zaloPayConfig.BaseUrl, param);


            CreatePaymentResponse createPaymentResponse = new CreatePaymentResponse()
            {
                Message = "Đang tiến hành thanh toán ZaloPay"
            };
            createPaymentResponse.DisplayType = CreatePaymentReturnType.Url;
            foreach (var entry in result)
            {
                if (entry.Key == "orderurl")
                {
                    createPaymentResponse.Url = entry.Value.ToString();
                }
                else if (entry.Key == "returnmessage")
                {
                    createPaymentResponse.Message = entry.Value.ToString();
                }
            }
            return createPaymentResponse;
        }
    }
}
