using System;
using System.Collections.Generic;

namespace ResoPayment.Infrastructure.Models
{
    public partial class PaymentProvider
    {
        public PaymentProvider()
        {
            BrandPaymentProviderMappings = new HashSet<BrandPaymentProviderMapping>();
            Transactions = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? PicUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = null!;

        public virtual ICollection<BrandPaymentProviderMapping> BrandPaymentProviderMappings { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
