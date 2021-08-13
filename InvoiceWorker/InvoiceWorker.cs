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
        private readonly ILogger _logger;

        public InvoiceWorker(HttpClient httpClient, IInvoiceHandler invoiceHandler, IHostApplicationLifetime appLifetime, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _invoiceHandler = invoiceHandler;
            _appLifetime = appLifetime;
            _logger = loggerFactory.CreateLogger<InvoiceWorker>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<InvoiceEventResponse>(await InvoiceApiSettings.GenerateUriAsync(), JsonSerializerOptions(), stoppingToken);
                    if (!response.Items?.Any() ?? true)
                    {
                        _logger.LogInformation("Invoice Worker has not received Invoice data events. The application will exit.");
                        _appLifetime.StopApplication();
                        return;
                    }

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

        private static JsonSerializerOptions JsonSerializerOptions()
        {
            var options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }
    }

    public class InvoiceEventResponse
    {
        public IEnumerable<InvoiceEvent> Items { get; set; }
    }
}
