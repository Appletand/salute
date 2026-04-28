using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Salute.Data;
using Salute.Data.Repositories;
using Salute.Interfaces;
using Salute.Services;
using DotNetEnv;
using System;

namespace Salute;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        try
        {
            Env.Load();
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                ?? "Host=localhost;Database=SaluteDB;Username=postgres;Password=postgres";

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IAnamnesisRepository, AnamnesisRepository>();

            builder.Services.AddTransient<ViewModels.PatientsViewModel>();
            builder.Services.AddTransient<ViewModels.AnamnesisViewModel>();
            builder.Services.AddTransient<ViewModels.PatientsViewModel>();
            builder.Services.AddTransient<ViewModels.AnamnesisViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO FATAL NA INICIALIZAÇÃO: {ex.Message}");
            Console.WriteLine($"{ex.InnerException?.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }
}