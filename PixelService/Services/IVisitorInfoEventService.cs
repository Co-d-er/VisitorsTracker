namespace PixelService.Services;

public interface IVisitorInfoEventService
{
    public Task Send(HttpContext context);
}