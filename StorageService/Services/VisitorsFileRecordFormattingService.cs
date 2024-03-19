using System.Text;

namespace StorageService.Services;

public class VisitorsFileRecordFormattingService : IFileRecordFormattingService
{
    public string Format(params string[] recordParts)
    {
        if (recordParts == null) throw new ArgumentNullException(nameof(recordParts));

        const string recordPartSeparator = "|";
        var fileRecord = new StringBuilder();

        foreach (var recordPart in recordParts)
        {
            var currentRecordPart = string.IsNullOrEmpty(recordPart) ? "null" : recordPart;

            if (fileRecord.Length == 0)
            {
                fileRecord.Append(currentRecordPart);
                continue;
            }
            
            fileRecord.Append(recordPartSeparator + currentRecordPart);
        }

        return fileRecord.ToString();
    }
}