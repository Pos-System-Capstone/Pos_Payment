﻿using System;
using System.Collections.Generic;

namespace ResoPayment.Infrastructure.Models
{
    public partial class Transaction
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid StoreId { get; set; }
        public Guid AccountId { get; set; }
        public double Amount { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public string? Notes { get; set; }
        public string Status { get; set; } = null!;
        public string TransactionCode { get; set; } = null!;
        public Guid PaymentProviderId { get; set; }
        public Guid OrderId { get; set; }

        public virtual Brand Brand { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
        public virtual PaymentProvider PaymentProvider { get; set; } = null!;
        public virtual Store Store { get; set; } = null!;
    }
}
