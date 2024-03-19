using System.Globalization;

namespace StorageService.Services;

public class DateTimeConvertingService : IDateTimeConvertingService
{
    public string ConvertToUtcIso8601(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffZ", CultureInfo.InvariantCulture);
    }
}