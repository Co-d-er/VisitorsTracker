namespace StorageService.Handlers;

public interface IEventHandler
{
    public Task HandleAsync(string  @event);
}