using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Extensions;
using ResoPayment.Infrastructure.Models;
using ResoPayment.Infrastructure;
using ResoPayment.Payload.Request;
using ResoPayment.Payload.Response;
using ResoPayment.Service.Interfaces;
using Newtonsoft.Json;
using ZaloPay.Helper.Crypto;
using ZaloPay.Helper;
using System.Collections.Generic;

namespace ResoPayment.Service.Implements
{
    public class ZaloPayService : BaseService<ZaloPayService>, IZaloPayServices
    {
        private readonly IConfiguration _configuration;

        public ZaloPayService(IUnitOfWork<PosPaymentContext> unitOfWork, ILogger<VnPayService> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, httpContextAccessor)
        {
            _configuration = configuration;
        }
        public async Task<Dictionary<string, object>> CreatePayment(HttpContext context)
        {
            //Guid storeId = Guid.Parse(GetStoreIdFromJwt());
            string appid = "2554";
            string key1 = "sdngKKJmqEMzvh5QQcdD2A9XBSKUNaYn";
            string createOrderUrl = "https://sandbox.zalopay.com.vn/v001/tpe/createorder";


            var transid = Guid.NewGuid().ToString();
            var embeddata = new { merchantinfo = "embeddata123" };
            var items = new[]{
                new { itemid = "knb", itemname = "kim nguyen bao", itemprice = 198400, itemquantity = 1 } };

            var param = new Dictionary<string, string>();
            param.Add("appid", appid);
            param.Add("appuser", "demo");
            param.Add("apptime", Utils.GetTimeStamp().ToString());
            param.Add("amount", "50000");
            param.Add("apptransid", DateTime.Now.ToString("yyMMdd") + "_" + transid); // mã giao dich có định dạng yyMMdd_xxxx
            param.Add("embeddata", JsonConvert.SerializeObject(embeddata));
            param.Add("item", JsonConvert.SerializeObject(items));
            param.Add("description", "ZaloPay demo");
            param.Add("bankcode", "zalopayapp");

            var data = appid + "|" + param["apptransid"] + "|" + param["appuser"] + "|" + param["amount"] + "|"
                + param["apptime"] + "|" + param["embeddata"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, data));

            var result = await HttpHelper.PostFormAsync(createOrderUrl, param);
            return result;
        }
    }
}
