using System;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using InvoiceWorker.Models;
using Microsoft.Extensions.Logging;

namespace InvoiceWorker
{
    public class InvoiceWorker : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly IInvoiceHandler _invoiceHandler;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger<InvoiceWorker> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public InvoiceWorker(HttpClient httpClient, IInvoiceHandler invoiceHandler, IHostApplicationLifetime appLifetime, ILogger<InvoiceWorker> logger)
        {
            _httpClient = httpClient;
            _invoiceHandler = invoiceHandler;
            _appLifetime = appLifetime;
            _logger = logger;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<InvoiceEventResponse>(await InvoiceApiSettings.GenerateUriAsync(), _jsonSerializerOptions, stoppingToken);

                    await InvoiceApiSettings.SaveHighwatermarkAsync(response.Items.Last().Id);
                    foreach (var invoiceEvent in response.Items)
                    {
                        await _invoiceHandler.ProcessEventAsync(invoiceEvent);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "Invoice Worker fatal error. The application will exit.");
                    _appLifetime.StopApplication();
                }
            }
        }
    }

    public class InvoiceEventResponse
    {
        public IEnumerable<InvoiceEvent> Items { get; set; }
    }
}
