using System;
using System.Collections.Generic;

namespace ResoPayment.Infrastructure.Models
{
    public partial class Order
    {
        public Guid Id { get; set; }
        public string InvoiceId { get; set; } = null!;
        public double TotalAmount { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Guid StoreId { get; set; }

        public virtual Store Store { get; set; } = null!;
        public virtual Transaction? Transaction { get; set; }
    }
}
