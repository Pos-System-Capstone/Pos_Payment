using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResoPayment.Infrastructure.PaymentConfigModels
{
    public class ZaloPayConfig
    {
        public string BaseUrl { get; set; }
        public string AppId { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string BankCode { get; set; }
    }
}
