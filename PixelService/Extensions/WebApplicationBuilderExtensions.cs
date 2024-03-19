using PixelService.Options;
using PixelService.Services;
using StackExchange.Redis;

namespace PixelService.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRedisService, RedisService>();
        builder.Services.AddScoped<IDateTimeService, DateTimeService>();
        builder.Services.AddScoped<ITransparentImageService, TransparentImageService>();
        builder.Services.AddScoped<IVisitorInfoEventService, VisitorInfoEventService>();

        builder.Services.AddLogging(loggerBuilder => loggerBuilder.AddConsole());
        
        return builder; 
    }

    public static WebApplicationBuilder RegisterOptions(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<RedisOptions>().BindConfiguration(nameof(RedisOptions));
        
        return builder; 
    }

    public static WebApplicationBuilder ConfigureRedis(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDatabase>(sp =>
        {
            const string connectionStringKey = "RedisOptions:ConnectionString";
            var connectionString = builder.Configuration[connectionStringKey];
            var redisOptions = ConfigurationOptions.Parse(connectionString);
            redisOptions.AbortOnConnectFail = false;
            var connectionMultiplexer = ConnectionMultiplexer.Connect(redisOptions);
            var redis = connectionMultiplexer.GetDatabase();
            return redis;
        });

        return builder;
    }
}