using System;
using System.Collections.Generic;

namespace ResoPayment.Infrastructure.Models
{
    public partial class Order
    {
        public Guid Id { get; set; }
        public string InvoiceId { get; set; } = null!;
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double FinalAmount { get; set; }
        public double Vat { get; set; }
        public double Vatamount { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Guid StoreId { get; set; }

        public virtual Store Store { get; set; } = null!;
    }
}
