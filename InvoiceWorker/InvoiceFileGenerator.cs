using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InvoiceWorker.Models;

namespace InvoiceWorker
{
    public interface IInvoiceGenerator
    {
        Task AddRecordAsync(InvoiceEvent record);
    }
    
    public class InvoiceFileGenerator : IInvoiceGenerator
    {
        private string GetInvoiceFilePath(Guid invoiceId) => Path.Combine(Directory.GetCurrentDirectory(), $"InvoiceData-{invoiceId}.txt");

        public async Task AddRecordAsync(InvoiceEvent invoiceEvent)
        {
            var lines = new List<string>
            {
                $"Invoice Number: {invoiceEvent.Content.InvoiceNumber}",
                $"Status: {invoiceEvent.Content.Status}",
                $"Created Date: {invoiceEvent.Content.CreatedDateUtc}",
                $"Due Date: {invoiceEvent.Content.DueDateUtc}",
                Environment.NewLine
            };

            lines.AddRange(invoiceEvent.Content.LineItems.SelectMany(li =>
                new[]
                {
                    $"Item description: {li.Description}",
                    $"Item quantity: {li.Quantity}",
                    $"Item cost: {li.UnitCost}",
                    $"Item total cost: {li.LineItemTotalCost}",
                    Environment.NewLine
                }));

            await File.WriteAllLinesAsync(GetInvoiceFilePath(invoiceEvent.Content.InvoiceId), lines);
        }
    }
}
