using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using InvoiceWorker.Models;

namespace InvoiceWorker
{
    public class InvoiceWorker : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly IInvoiceHandler _invoiceHandler;

        public InvoiceWorker(HttpClient httpClient, IInvoiceHandler invoiceHandler)
        {
            _httpClient = httpClient;
            _invoiceHandler = invoiceHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _httpClient.GetFromJsonAsync<InvoiceEventResponse>(await InvoiceApiSettings.GenerateUriAsync(), stoppingToken);
                await InvoiceApiSettings.SaveHighwatermarkAsync(response.Items.Last().Id);

                foreach (var invoiceEvent in response.Items)
                {
                    await _invoiceHandler.ProcessEventAsync(invoiceEvent);
                }
            }
        }
    }

    public class InvoiceEventResponse
    {
        public IEnumerable<InvoiceEvent> Items { get; set; }
    }
}
