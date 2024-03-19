using PixelService.Extensions;

var app = WebApplication.CreateBuilder(args)
    .RegisterServices()
    .RegisterOptions()
    .ConfigureRedis()
    .Build();

app.UseHttpsRedirection();
app.RegisterEndpoints();
app.Run();