namespace StorageService.Events;

public record VisitorInfoEvent(DateTime DateTimeInUtc, string Referrer, string UserAgent, string IpAddress);