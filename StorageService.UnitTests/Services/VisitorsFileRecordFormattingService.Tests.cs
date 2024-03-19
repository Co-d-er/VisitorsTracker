using StorageService.Services;
using Xunit;

namespace StorageService.Tests.Services;

public class VisitorsFileRecordFormattingServiceTests
{
    private readonly IFileRecordFormattingService _visitorsFileRecordFormattingService = new VisitorsFileRecordFormattingService();

    [Fact]
    public void Format_ReturnEmptyString_IfThereIsNoParameter()
    {
        // Act
        var actual = _visitorsFileRecordFormattingService.Format();

        // Assert
        Assert.Equal(string.Empty, actual);
    }
    
    [Fact]
    public void Format_ThrowsArgumentNullException_IfThereIsOnlyOneNullParameter()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _visitorsFileRecordFormattingService.Format(null));
    }

    [Theory]
    [InlineData("2022-12-19T14:16:49.9605280Z|https://google.com|SomeUserAgent 1.2.3|192.168.1.1","2022-12-19T14:16:49.9605280Z", "https://google.com", "SomeUserAgent 1.2.3", "192.168.1.1")]
    [InlineData("someValue|null", "someValue", null)]
    [InlineData("null", "")]
    [InlineData("null|someValue", null, "someValue")]
    [InlineData("null|someValue", "", "someValue")]
    [InlineData("someValue", "someValue")]
    public void Format_FormatsRecordPartsCorrectly(string expected, params string[] recordParts)
    {
        // Act
        var actual = _visitorsFileRecordFormattingService.Format(recordParts);

        // Assert
        Assert.Equal(expected, actual);
    }
}