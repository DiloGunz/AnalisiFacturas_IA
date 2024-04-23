using ADIA.Web.Components;
using ADIA.Web.Config;
using ADIA.Web.Middlewares;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Blazorise.Tests.bUnit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfig(builder.Configuration);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBlazoriseTests()
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseMiddleware<UnhandledExceptionMiddleware>();

await app.RunAsync();
