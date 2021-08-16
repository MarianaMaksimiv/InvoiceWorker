using System.IO;
using System.Threading.Tasks;

namespace InvoiceWorker
{
    public class InvoiceApiSettings
    {
        private const string InvoiceApiUri = "https://invoice-worker-api-mock.netlify.app/api/invoices/events";
        private static readonly string LastProcessedEventFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        private static readonly string LastProcessedEventFilePath = Path.Combine(LastProcessedEventFolder, "InvoiceWorkerLastProcessedEvent.txt");

        public static async Task<string> GenerateUriAsync()
        {
            if (File.Exists(LastProcessedEventFilePath) && long.TryParse(await File.ReadAllTextAsync(LastProcessedEventFilePath), out var eventId))
            {
                return $"{InvoiceApiUri}?afterEventId={eventId}";
            }

            return InvoiceApiUri;
        }

        public static async Task SaveLastProcessedEventIdAsync(long eventId)
        {
            Directory.CreateDirectory(LastProcessedEventFolder);
            await File.WriteAllTextAsync(LastProcessedEventFilePath, eventId.ToString());
        }
    }
}
