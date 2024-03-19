using Newtonsoft.Json;
using StackExchange.Redis;

namespace PixelService.Services;

public class RedisService : IRedisService
{
    private readonly IDatabase _redis;

    public RedisService(IDatabase redis)
    {
        _redis = redis;
    }

    public async Task Publish<TEvent>(string channel, TEvent @event)
        where TEvent : class
    {
        string message = JsonConvert.SerializeObject(@event);
        await _redis.PublishAsync(channel, message);
    }
}