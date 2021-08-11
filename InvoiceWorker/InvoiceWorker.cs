using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        private readonly ILogger<InvoiceWorker> _logger;

        public InvoiceWorker(HttpClient httpClient, IInvoiceHandler invoiceHandler, ILogger<InvoiceWorker> logger)
        {
            _httpClient = httpClient;
            _invoiceHandler = invoiceHandler;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("InvoiceWorker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

                var response = await _httpClient.GetFromJsonAsync<InvoiceEventResponse>(InvoiceApiSettings.GenerateUri());

                foreach (var invoiceEvent in response.Items)
                {
                    InvoiceApiSettings.SaveHighwatermark(invoiceEvent.Id);
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
