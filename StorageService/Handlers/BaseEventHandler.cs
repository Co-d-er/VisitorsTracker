using Newtonsoft.Json;

namespace StorageService.Handlers;

public abstract class BaseEventHandler<TEvent>
    where TEvent: class
{
    public async Task HandleAsync(string @event)
    {
        var deserializedEvent = JsonConvert.DeserializeObject<TEvent>(@event);
        await HandleAsync(deserializedEvent);
    }

    protected abstract Task HandleAsync(TEvent @event);
}