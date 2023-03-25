using System;
using System.Collections.Generic;

namespace ResoPayment.Infrastructure.Models
{
    public partial class Brand
    {
        public Brand()
        {
            BrandPaymentProviderMappings = new HashSet<BrandPaymentProviderMapping>();
            Stores = new HashSet<Store>();
            Transactions = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Status { get; set; } = null!;
        public string? Vatcode { get; set; }
        public string? PgppublicKey { get; set; }
        public string? PgpprivateKey { get; set; }
        public string? Pgppassword { get; set; }
        public string? DesKey { get; set; }
        public string? DesVector { get; set; }
        public string? AccessToken { get; set; }
        public string? TaxCode { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual ICollection<BrandPaymentProviderMapping> BrandPaymentProviderMappings { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
