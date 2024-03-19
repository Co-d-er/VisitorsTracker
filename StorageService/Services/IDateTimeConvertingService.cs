namespace StorageService.Services;

public interface IDateTimeConvertingService
{
    public string ConvertToUtcIso8601(DateTime dateTime);
}