namespace PixelService.Events;

public record VisitorInfoEvent(DateTime DateTimeInUtc, string Referrer, string UserAgent, string IpAddress);