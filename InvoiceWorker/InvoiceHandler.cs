using System.Threading.Tasks;
using InvoiceWorker.Models;

namespace InvoiceWorker
{
    public interface IInvoiceHandler
    {
        Task ProcessEventAsync(InvoiceEvent @event);
    }

    public class InvoiceHandler : IInvoiceHandler
    {
        private readonly IInvoiceGenerator _invoiceGenerator;

        public InvoiceHandler(IInvoiceGenerator invoiceGenerator)
        {
            _invoiceGenerator = invoiceGenerator;
        }

        public async Task ProcessEventAsync(InvoiceEvent @event)
        {
            await _invoiceGenerator.AddRecordAsync(@event);
        }
    }
}
