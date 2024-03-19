using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PixelService.Services;

public class TransparentImageService : ITransparentImageService
{
    private byte[] CachedImage { get; set; }
    
    public byte[] Create()
    {
        if (CachedImage is { Length: > 0 })
        {
            return CachedImage;
        }

        using MemoryStream stream = new MemoryStream();
        
        using (Image<Rgba32> image = new Image<Rgba32>(1, 1))
        {
            image[0, 0] = new Rgba32(0, 0, 0, 0);
            image.SaveAsGif(stream);
        }
        
        CachedImage = stream.ToArray();

        return CachedImage;
    }
}