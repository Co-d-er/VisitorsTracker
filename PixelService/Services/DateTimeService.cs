namespace PixelService.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime GetDateTimeInUtcNow()
    {
        return DateTime.UtcNow;
    }
}