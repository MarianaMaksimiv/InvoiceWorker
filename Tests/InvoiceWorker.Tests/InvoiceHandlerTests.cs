using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using InvoiceWorker.Models;
using Xunit;

namespace InvoiceWorker.Tests
{
    public class InvoiceHandlerTests
    {
        private readonly InvoiceHandler _handler;

        public InvoiceHandlerTests()
        {
            _handler = new InvoiceHandler(new InvoiceFileGenerator());
        }

        [Fact]
        public async Task ProcessEventAsync_ShouldAddRecord()
        {
            // Arrange
            var @event = new InvoiceEvent
            {
                Content = new Invoice
                {
                    InvoiceId = Guid.NewGuid(),
                    InvoiceNumber = "INV-Test-001",
                    Status = "DRAFT",
                    CreatedDateUtc = new DateTime(2021, 3, 22, 19, 15, 0, DateTimeKind.Utc),
                    DueDateUtc = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    LineItems = new []
                    {
                        new InvoiceLineItem { Description = "Xero Supplier", Quantity = 2, UnitCost = 24.25m, LineItemTotalCost = 48.5m }
                    }
                }
            };

            var expectedFileOutput =
$@"Invoice Number: INV-Test-001
Status: DRAFT
Created Date: 22/03/2021 7:15:00 PM
Due Date: 1/01/2022 12:00:00 AM


Item description: Xero Supplier
Item quantity: 2
Item cost: 24.25
Item total cost: 48.5


";

            // Act
            await _handler.ProcessEventAsync(@event);

            // Assert
            var invoiceFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Invoices", $"Invoice-{@event.Content.InvoiceId}.txt");
            File.Exists(invoiceFilePath).Should().BeTrue();
            var result = await File.ReadAllTextAsync(invoiceFilePath);
            result.Should().BeEquivalentTo(expectedFileOutput);
        }
    }
}
