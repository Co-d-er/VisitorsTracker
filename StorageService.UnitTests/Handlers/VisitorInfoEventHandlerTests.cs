using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using StorageService.Events;
using StorageService.Handlers;
using StorageService.Services;
using Xunit;

namespace StorageService.Tests.Handlers;

public class VisitorInfoEventHandlerTests
{
    private const string Referrer = "http://example.com";
    private const string UserAgent = "Test UserAgent";
    private const string IpAddress = "192.168.1.1";
    
    private readonly Mock<IFileWritingService> _fileWritingServiceMock;
    private readonly Mock<IFileRecordFormattingService> _fileRecordFormattingServiceMock;
    private readonly Mock<IDateTimeConvertingService> _dateTimeConvertingServiceMock;
    private readonly VisitorInfoEventHandler _visitorInfoEventHandler;

    public VisitorInfoEventHandlerTests()
    {
        _fileWritingServiceMock = new Mock<IFileWritingService>();
        _fileRecordFormattingServiceMock = new Mock<IFileRecordFormattingService>();
        _dateTimeConvertingServiceMock = new Mock<IDateTimeConvertingService>();
        var loggerMock = new Mock<ILogger<VisitorInfoEventHandler>>();

        _visitorInfoEventHandler = new VisitorInfoEventHandler(
            _fileWritingServiceMock.Object,
            _fileRecordFormattingServiceMock.Object,
            _dateTimeConvertingServiceMock.Object,
            loggerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ThrowsArgumentNullException_WhenIpAddressIsNull()
    {
        // Arrange
        var @event = new VisitorInfoEvent(DateTime.UtcNow, Referrer, UserAgent, null);
        var serializedEvent = JsonConvert.SerializeObject(@event);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _visitorInfoEventHandler.HandleAsync(serializedEvent));
    }

    [Fact]
    public async Task HandleAsync_CallsServicesWithCorrectParameters()
    {
        // Arrange
        var dateTime = DateTime.UtcNow;
        var @event = new VisitorInfoEvent(dateTime, Referrer, UserAgent, IpAddress);
        var serializedEvent = JsonConvert.SerializeObject(@event);
        
        _fileWritingServiceMock.Setup(s => s.AppendLine(It.IsAny<string>()));

        const string formattedRecord = "formattedRecord";
        _fileRecordFormattingServiceMock.Setup(s => s.Format(It.IsAny<string[]>())).Returns(formattedRecord);
        
        var formattedDateTime = "2022-12-19T14:16:49.9605280Z";
        _dateTimeConvertingServiceMock.Setup(s => s.ConvertToUtcIso8601(It.IsAny<DateTime>())).Returns(formattedDateTime);

        // Act
        await _visitorInfoEventHandler.HandleAsync(serializedEvent);

        // Assert
        _fileRecordFormattingServiceMock.Verify(s => s.Format(formattedDateTime, @event.Referrer, @event.UserAgent, @event.IpAddress), Times.Once);
        _fileWritingServiceMock.Verify(s => s.AppendLine(formattedRecord), Times.Once);
        _dateTimeConvertingServiceMock.Verify(d => d.ConvertToUtcIso8601(dateTime), Times.Once);
    }
}