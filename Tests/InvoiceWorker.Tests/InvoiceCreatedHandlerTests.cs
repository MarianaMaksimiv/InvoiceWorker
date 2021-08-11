using InvoiceWorker.Models;
using Moq;
using Xunit;

namespace InvoiceWorker.Tests
{
    public class InvoiceCreatedHandlerTests
    {
        private readonly Mock<IInvoiceGenerator> _invoiceGeneratorMock = new();
        private readonly InvoiceHandler _handler;

        public InvoiceCreatedHandlerTests()
        {
            _handler = new InvoiceHandler(_invoiceGeneratorMock.Object);
        }

        [Fact]
        public void ProcessEventAsync_ShouldAddRecord()
        {
            // Arrange
            var @event = new InvoiceEvent();

            // Act
            _handler.ProcessEventAsync(@event);

            // Assert
            _invoiceGeneratorMock.Verify(x => x.AddRecord(@event), Times.Once);
        }
    }
}
