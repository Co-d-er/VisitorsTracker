using StorageService.Events;
using StorageService.Services;

namespace StorageService.Handlers;

public class VisitorInfoEventHandler : BaseEventHandler<VisitorInfoEvent>, IEventHandler
{
    private readonly IFileWritingService _fileWritingService;
    private readonly IFileRecordFormattingService _fileRecordFormattingService;
    private readonly IDateTimeConvertingService _dateTimeConvertingService;
    private readonly ILogger<VisitorInfoEventHandler> _logger;

    public VisitorInfoEventHandler(
        IFileWritingService fileWritingService,
        IFileRecordFormattingService fileRecordFormattingService,
        IDateTimeConvertingService dateTimeConvertingService,
        ILogger<VisitorInfoEventHandler> logger)
    {
        _fileWritingService = fileWritingService;
        _fileRecordFormattingService = fileRecordFormattingService;
        _dateTimeConvertingService = dateTimeConvertingService;
        _logger = logger;
    }

    protected override Task HandleAsync(VisitorInfoEvent @event)
    {
        if (@event.IpAddress == null)
        {
            throw new ArgumentNullException($"The following event {@event} can not be processed as the {nameof(@event.IpAddress)} is not defined");
        }
        
        string dateTimeInUtc = _dateTimeConvertingService.ConvertToUtcIso8601(@event.DateTimeInUtc);
        var newFileRecord = _fileRecordFormattingService.Format(dateTimeInUtc, @event.Referrer, @event.UserAgent, @event.IpAddress);

        _fileWritingService.AppendLine(newFileRecord);
        
        return Task.CompletedTask;
    }
}