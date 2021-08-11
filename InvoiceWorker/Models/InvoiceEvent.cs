using System;

namespace InvoiceWorker.Models
{
    public class InvoiceEvent
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public Invoice Content { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}
