using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ResoPayment.Helpers;
using ResoPayment.Infrastructure.PaymentConfigModels;
using ResoPayment.Payload.Response;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http.Headers;
using ZaloPay.Helper;

namespace ResoPayment.PaymentStrategy.PaymentStrategies
{
    public class VietQRPaymentStrategy : IPaymentStrategy
    {
        private readonly VietQRConfig _vietQRConfig;
        private readonly string _orderDescription;
        private readonly double _amount;
        public VietQRPaymentStrategy(string VietQRConfigJson, string orderDescription, double amount)
        {

            _orderDescription = orderDescription;
            _amount = amount;
            _vietQRConfig = JsonConvert.DeserializeObject<VietQRConfig>(VietQRConfigJson) ?? throw new InvalidOperationException();
        }

        public async Task<CreatePaymentResponse> ExecutePayment()
        {
            HttpClient client = new HttpClient();

            //Add header
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-client-id", _vietQRConfig.ClientId);
            client.DefaultRequestHeaders.Add("x-api-key", _vietQRConfig.ApiKey);

            //Add request body
            var values = new Dictionary<string, string>();
            values.Add("accountNo", _vietQRConfig.AccountNo);
            values.Add("accountName", _vietQRConfig.AccountName);
            values.Add("acqId", _vietQRConfig.BankCode);
            values.Add("amount", _amount.ToString());
            values.Add("addInfo", _orderDescription);
            values.Add("template", "print");
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(_vietQRConfig.BaseUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);

            CreatePaymentResponse createPaymentResponse = new CreatePaymentResponse();
            createPaymentResponse.DisplayType = "QR";
            foreach (var entry in result)
            {
                if (entry.Key == "desc")
                {
                    createPaymentResponse.Message = entry.Value.ToString();
                }
                else if (entry.Key == "data")
                {
                    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(entry.Value.ToString());
                    foreach (var item in data)
                    {
                        if (item.Key == "qrCode")
                        {
                            createPaymentResponse.Url = item.Value.ToString();
                        }
                    }
                }
                return createPaymentResponse;
            }
        }
    }
}
