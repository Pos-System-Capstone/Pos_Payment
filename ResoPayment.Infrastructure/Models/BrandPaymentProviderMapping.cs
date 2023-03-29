using System;
using System.Collections.Generic;

namespace ResoPayment.Infrastructure.Models
{
    public partial class BrandPaymentProviderMapping
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid PaymentProviderId { get; set; }
        public string? Config { get; set; }
        public string Status { get; set; } = null!;

        public virtual Brand Brand { get; set; } = null!;
        public virtual PaymentProvider PaymentProvider { get; set; } = null!;
    }
}
