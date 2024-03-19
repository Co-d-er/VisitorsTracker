using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PixelService.Events;
using PixelService.Services;
using PixelService.UnitTests.Consts;
using Xunit;

namespace PixelService.UnitTests.Services;

public class VisitorInfoEventServiceTests
{
    private readonly Mock<IDateTimeService> _dateTimeServiceMock;
    private readonly Mock<IRedisService> _redisServiceMock;
    private readonly IVisitorInfoEventService _visitorInfoEventService;

    public VisitorInfoEventServiceTests()
    {
        _dateTimeServiceMock = new Mock<IDateTimeService>();
        _redisServiceMock = new Mock<IRedisService>();
        var loggerMock = new Mock<ILogger<VisitorInfoEventService>>();
        
        _visitorInfoEventService = new VisitorInfoEventService(
            _redisServiceMock.Object,
            _dateTimeServiceMock.Object,
            loggerMock.Object);
    }

    [Fact]
    public async Task Send_WithValidHttpContext_SendsEventToRedis()
    {
        // Arrange
        var mockHttpContext = new DefaultHttpContext();
        mockHttpContext.Request.Headers["Referer"] = StubConstants.Referrer;
        mockHttpContext.Request.Headers["User-Agent"] = StubConstants.UserAgent;
        mockHttpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse(StubConstants.IpAddress);
        var dateTimeInUtc = DateTime.UtcNow;
        
        _dateTimeServiceMock.Setup(s => s.GetDateTimeInUtcNow()).Returns(dateTimeInUtc);

        // Act
        await _visitorInfoEventService.Send(mockHttpContext);

        // Assert
        var expectedEvent = new VisitorInfoEvent(dateTimeInUtc, StubConstants.Referrer, StubConstants.UserAgent, StubConstants.IpAddress);
        _redisServiceMock.Verify(
            rs => rs.Publish(nameof(VisitorInfoEvent), It.Is<VisitorInfoEvent>(v => v.Equals(expectedEvent))),
            Times.Once);
    }

    [Fact]
    public async Task Send_WithNullRemoteIpAddress_LogsInformationAndDoesNotSendEventToRedis()
    {
        // Arrange
        var mockHttpContext = new DefaultHttpContext();

        // Act
        await _visitorInfoEventService.Send(mockHttpContext);

        // Assert
        _redisServiceMock.Verify(
            rs => rs.Publish(It.IsAny<string>(), It.IsAny<VisitorInfoEvent>()),
            Times.Never);
    }
}