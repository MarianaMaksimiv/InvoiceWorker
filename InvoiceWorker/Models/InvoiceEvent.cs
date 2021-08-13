using System;

namespace InvoiceWorker.Models
{
    public class InvoiceEvent
    {
        public long Id { get; set; }
        public EventType Type { get; set; }
        public Invoice Content { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }

    public enum EventType
    {
        INVOICE_CREATED
    }
}
