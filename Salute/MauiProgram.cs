using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Salute.Data;
using Salute.Data.Repositories;
using Salute.Interfaces;
using Salute.Services;
using Salute.ViewModels;
using Salute.Views.Patients;
using Salute.Views.Anamnesis;
using DotNetEnv;

namespace Salute;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        try
        {
            var envPath = Path.Combine(AppContext.BaseDirectory, ".env");
            Env.Load(envPath);

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

            // Database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Repositories
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IAnamnesisRepository, AnamnesisRepository>();

            // Services
            builder.Services.AddScoped<IGeminiService, GeminiService>();

            // ViewModels
            builder.Services.AddTransient<PatientsViewModel>();
            builder.Services.AddSingleton<NavigationState>();
            builder.Services.AddTransient<PatientDetailViewModel>();
            builder.Services.AddTransient<AddPatientViewModel>();
            builder.Services.AddTransient<AnamnesisViewModel>();

            // Pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<PatientsPage>();
            builder.Services.AddTransient<PatientDetailPage>();
            builder.Services.AddTransient<AddPatientPage>();
            builder.Services.AddTransient<AnamnesisPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO FATAL NA INICIALIZAÇÃO: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }
}