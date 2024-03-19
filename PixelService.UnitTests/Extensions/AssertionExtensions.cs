using FluentAssertions;

namespace PixelService.UnitTests.Extensions;

public static class AssertionExtensions
{
    public static bool Assert<TObject>(this TObject actual, TObject expected)
    {
        actual.Should().BeEquivalentTo(expected);
        return true;
    }
}