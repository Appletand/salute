using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SaluteWeb;
using SaluteWeb.Interfaces;
using SaluteWeb.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Hardening: Resolve a URL dinamicamente via appsettings.{Env}.json
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? builder.HostEnvironment.BaseAddress;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

// Injeção de Dependência (DI)
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IAnamnesisRepository, AnamnesisRepository>();

await builder.Build().RunAsync();