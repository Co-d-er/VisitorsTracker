using StorageService.Extensions;

WebApplication.CreateBuilder(args)
    .RegisterServices()
    .RegisterOptions()
    .ConfigureRedis()
    .Build()
    .AddEventHandlers()
    .Run();