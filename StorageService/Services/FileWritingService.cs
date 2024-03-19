using Microsoft.Extensions.Options;
using StorageService.Options;

namespace StorageService.Services;

public sealed class FileWritingService : IFileWritingService, IDisposable
{
    private readonly object _lockObject = new object();
    private readonly StreamWriter _streamWriter;

    public FileWritingService(IOptions<VisitsFileOptions> options)
    {
        var filePath = options.Value.Path;
        var r = Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        var fullPath = Path.GetFullPath(filePath); 
        _streamWriter = new StreamWriter(fullPath, append: true)
        {
            AutoFlush = true
        };
    }

    public void AppendLine(string text)
    {
        lock (_lockObject)
        {
            _streamWriter.WriteLine(text);
        }
    }

    public void Close()
    {
        lock (_lockObject)
        {
            _streamWriter?.Close();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            lock (_lockObject)
            {
                _streamWriter?.Dispose();
            }
        }
    }
}