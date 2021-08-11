using System;

namespace InvoiceWorker.Models
{
    public class InvoiceLineItem
    {
        public Guid LineItemId { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineItemTotalCost { get; set; }
    }
}
