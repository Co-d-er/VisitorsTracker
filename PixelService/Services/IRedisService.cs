namespace PixelService.Services;

public interface IRedisService
{
    public Task Publish<TEvent>(string channel, TEvent @event) where TEvent : class;
}