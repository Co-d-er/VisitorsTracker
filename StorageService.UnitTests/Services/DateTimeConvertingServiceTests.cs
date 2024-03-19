using StorageService.Services;
using Xunit;

namespace StorageService.Tests.Services;

public class DateTimeConvertingServiceTests
{
    private readonly IDateTimeConvertingService _dateTimeConvertingService = new DateTimeConvertingService();

    [Theory]
    [InlineData("2023-05-10T08:30:15.1230000Z", 2023, 5, 10, 8, 30, 15, 123)]
    public void ConvertToUtcIso8601_ConvertsDateTimeToUtcIso8601String(string expected, int year, int month, int day, int hour, int minute, int second, int millisecond)
    {
        // Arrange
        var dateTime = new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Utc);

        // Act
        string actual = _dateTimeConvertingService.ConvertToUtcIso8601(dateTime);

        // Assert
        Assert.Equal(expected, actual);
    }
}