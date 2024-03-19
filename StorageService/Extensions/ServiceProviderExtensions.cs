using StackExchange.Redis;
using StorageService.Handlers;

namespace StorageService.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider AddEventHandlers<THandler, TEvent>(this IServiceProvider serviceProvider, string? channelName = null)
        where THandler : IEventHandler
        where TEvent : class
    {
        var redis = serviceProvider.GetRequiredService<ConnectionMultiplexer>();
        var subscriber = redis.GetSubscriber();
        var eventHandler = serviceProvider.GetRequiredService<THandler>();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        var currentChannelName = channelName ?? typeof(TEvent).Name;

        subscriber.SubscribeAsync(currentChannelName, Handler);

        return serviceProvider;

        async void Handler(RedisChannel channel, RedisValue message)
        {
            try
            {
                await eventHandler.HandleAsync(message!);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error appeared. The message {message} from the {channel} is not processed. Message: {ex.Message}");
            }
        }
    }
}