namespace StorageService.Services;

public interface IFileRecordFormattingService
{
    public string Format(params string[] recordParts);
}