using System.IO;

namespace InvoiceWorker
{
    public class InvoiceApiSettings
    {
        private const string InvoiceApiUri = "https://invoice-worker-api-mock.netlify.app/api/invoices/events";
        private const string HighwatermarkFilePath = @"C:/InvoiceWorkerHighwatermark";

        public static string GenerateUri()
        {
            if (File.Exists(HighwatermarkFilePath) && long.TryParse(File.ReadAllText(HighwatermarkFilePath), out var highwatermark))
            {
                return $"{InvoiceApiUri}?afterEventId={highwatermark}";
            }

            return InvoiceApiUri;
        }

        public static void SaveHighwatermark(long highwatermark)
        {
            File.WriteAllText(HighwatermarkFilePath, highwatermark.ToString());
        }
    }
}
