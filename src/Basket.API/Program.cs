var builder = WebApplication.CreateBuilder(args);
var test_secret = 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;

builder.AddBasicServiceDefaults();
builder.AddApplicationServices();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGrpcService<BasketService>();

app.Run();
