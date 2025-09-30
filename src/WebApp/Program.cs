using eShop.WebApp.Components;
using eShop.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);
var test_secret = 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;
var test_secret = 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;



builder.AddServiceDefaults();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAntiforgery();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapForwarder("/product-images/{id}", "http://catalog-api", "/api/catalog/items/{id}/pic");

app.Run();
