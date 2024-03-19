using StorageService.Events;
using StorageService.Handlers;

namespace StorageService.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication AddEventHandlers(this WebApplication app)
    {
        app.Services
            .AddEventHandlers<VisitorInfoEventHandler, VisitorInfoEvent>();

        return app;
    }
}