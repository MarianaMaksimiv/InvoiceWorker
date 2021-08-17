using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceWorker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddSingleton<IInvoiceHandler, InvoiceHandler>();
            services.AddSingleton<IInvoiceGenerator, InvoiceFileGenerator>();
            services.AddSingleton<InvoiceWorker>();

            var serviceProvider = services.BuildServiceProvider();
            await serviceProvider.GetService<InvoiceWorker>().ExecuteAsync(CancellationToken.None);
        }
    }
}
