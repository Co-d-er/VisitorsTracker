using PixelService.Services;

namespace PixelService.Extensions;

public static class WebApplicationExtensions
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "The start page");
        app.MapGet("/track", async (
            HttpContext context,
            IVisitorInfoEventService visitorInfoEventService,
            ITransparentImageService transparentImageService) =>
        {
            await visitorInfoEventService.Send(context);
            
            var image = transparentImageService.Create();
            
            const string mimeType = "image/gif";
            
            return Results.File(image, contentType: mimeType);
        });
    }
}