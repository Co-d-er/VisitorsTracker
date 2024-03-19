using Moq;
using PixelService.Events;
using PixelService.Services;
using PixelService.UnitTests.Consts;
using StackExchange.Redis;
using Xunit;

namespace PixelService.UnitTests.Services;

public class RedisServiceTests
{
    private readonly Mock<IDatabase> _databaseMock;
    private readonly IRedisService _redisService;

    public RedisServiceTests()
    {
        _databaseMock = new Mock<IDatabase>();
        _redisService = new RedisService(_databaseMock.Object);
    }

    [Fact]
    public async Task Publish_SendsEventToRedisChannel()
    {
        // Arrange
        const string channel = "test-channel";
        
        var testEvent = new VisitorInfoEvent(
            DateTime.UtcNow,
            StubConstants.Referrer,
            StubConstants.UserAgent,
            StubConstants.IpAddress);

        var expectedMessage = "{\"DateTimeInUtc\":\"" + testEvent.DateTimeInUtc.ToString("o") +
                              "\",\"Referrer\":\"http://example.com\",\"UserAgent\":\"Test UserAgent\",\"IpAddress\":\"192.168.1.1\"}";

        // Act
        await _redisService.Publish(channel, testEvent);

        // Assert
        _databaseMock.Verify(
            db => db.PublishAsync(channel, expectedMessage, CommandFlags.None),
            Times.Once);
    }
}