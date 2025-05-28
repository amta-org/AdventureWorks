using Asp.Versioning.Builder;
using eShop.Catalog.API.Apis;
using eShop.Catalog.API.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

// Register purchase validation service
builder.Services.AddHttpClient<IPurchaseValidationService, PurchaseValidationService>();
builder.Services.AddScoped<IPurchaseValidationService, PurchaseValidationService>();
builder.Services.AddHttpContextAccessor();

var withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi(withApiVersioning);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseStatusCodePages();

app.MapCatalogApi();

app.UseDefaultOpenApi();
app.Run();
