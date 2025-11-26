var builder = WebApplication.CreateBuilder(args);

builder.AddBasicServiceDefaults();
builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

await app.RunAsync();

var test_secret = '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
