using System;
using System.Collections.Generic;

namespace InvoiceWorker.Models
{
    public class Invoice
    {
        public Guid InvoiceId { get; init; }
        public string InvoiceNumber { get; init; }
        public IEnumerable<InvoiceLineItem> LineItems { get; init; }
        public string Status { get; init; }
        public DateTime DueDateUtc { get; init; }
        public DateTime CreatedDateUtc { get; init; }
        public DateTime UpdatedDateUtc { get; init; }
    }
}
