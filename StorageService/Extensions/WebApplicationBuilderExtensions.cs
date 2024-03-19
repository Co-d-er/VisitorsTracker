using StackExchange.Redis;
using StorageService.Handlers;
using StorageService.Services;
using StorageService.Options;

namespace StorageService.Extensions;

public static class WebApplicationBuilderExtensions
{
    private static readonly IDictionary<string, Func<string, Task>> EventHandlers = new Dictionary<string, Func<string, Task>>();

    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IFileWritingService, FileWritingService>();
        builder.Services.AddSingleton<IFileRecordFormattingService, VisitorsFileRecordFormattingService>();
        builder.Services.AddSingleton<IDateTimeConvertingService, DateTimeConvertingService>();
        
        builder.Services.AddLogging(loggerBuilder => loggerBuilder.AddConsole());
        
        return builder; 
    }

    public static WebApplicationBuilder RegisterOptions(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<VisitsFileOptions>().BindConfiguration(nameof(VisitsFileOptions));
        
        return builder; 
    }
    
    public static WebApplicationBuilder ConfigureRedis(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
        {
            const string connectionStringKey = "RedisOptions:ConnectionString";
            var connectionString = builder.Configuration[connectionStringKey];
            var redisOptions = ConfigurationOptions.Parse(connectionString);
            redisOptions.AbortOnConnectFail = false;
            redisOptions.ConnectTimeout = 30000;
            return ConnectionMultiplexer.Connect(redisOptions);
        });
        
        builder.Services.AddSingleton<VisitorInfoEventHandler>();
        
        return builder;
    }
}