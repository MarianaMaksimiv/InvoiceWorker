using System;
using System.Collections.Generic;

namespace InvoiceWorker.Models
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public IEnumerable<InvoiceLineItem> LineItems { get; set; }
        public string Status { get; set; }
        public DateTime DueDateUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime UpdatedDateUtc { get; set; }
    }
}
