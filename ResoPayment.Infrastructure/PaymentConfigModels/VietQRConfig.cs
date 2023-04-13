using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResoPayment.Infrastructure.PaymentConfigModels
{
    public class VietQRConfig
    {
        public string BaseUrl { get; set; }
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string BankCode { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
    }
}
