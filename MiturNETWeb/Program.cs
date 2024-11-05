
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MiturNetShared.Helper;
using MiturNetShared.Services;
using MiturNetWeb.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


builder.Services.AddHttpClient<IBaseHttpClient, BaseHttpClient>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
    });

builder.Services.AddHttpClient<IBaseHttpClientOdoo, BaseHttpClientOdoo>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
    });


builder.Services.Configure<HttpClientOptions>(builder.Configuration.GetSection("HttpClientOptions"));
builder.Services.Configure<LoginOdoo>(builder.Configuration.GetSection("LoginOdoo"));

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddScoped<ExportServices>();
builder.Services.AddScoped<ExportToFile>();
builder.Services.AddScoped<HttpClientConfig>();
builder.Services.AddScoped<EmailSettings>();


builder.Services.AddScoped<SecurityService>();
builder.Services.AddScoped<SecurityServiceOdoo>();

builder.Services.AddTransient<HubConnectionBuilder>();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddTransient<ILocalStorage, MyLocalStorage>();

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();

app.UseEndpoints(endpoints =>
{
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");
});


app.Run();
