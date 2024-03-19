using PixelService.Events;

namespace PixelService.Services;

public class VisitorInfoEventService : IVisitorInfoEventService
{
    private const string Channel = nameof(VisitorInfoEvent);

    private readonly IRedisService _redisService;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<VisitorInfoEventService> _logger;

    public VisitorInfoEventService(IRedisService redisService, IDateTimeService dateTimeService, ILogger<VisitorInfoEventService> logger)
    {
        _redisService = redisService;
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task Send(HttpContext context)
    {
        const string referrerHeader = "Referer";
        const string userAgentHeader = "User-Agent";

        if (context.Connection.RemoteIpAddress is null)
        {
            _logger.LogInformation("The event can not be sent as the ip address is not defined");
            return;
        }

        var visitorInfoEvent = new VisitorInfoEvent(
            _dateTimeService.GetDateTimeInUtcNow(),
            context.Request.Headers[referrerHeader].ToString(),
            context.Request.Headers[userAgentHeader].ToString(),
            context.Connection.RemoteIpAddress!.ToString());

        try
        {
            await _redisService.Publish(Channel, visitorInfoEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Some error appeared while sending the message {visitorInfoEvent} to the {Channel} channel.");
        }
    }
}