using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DentalApp.Application.Common.Interfaces;
using DentalApp.Infrastructure.Data.Context;
using DentalApp.Infrastructure.Data.Repositories;
using DentalApp.Infrastructure.ExternalServices.GeminiService;
using DentalApp.Infrastructure.ExternalServices.MarkdownExport;
using DentalApp.Infrastructure.Security;
using DentalApp.Presentation.ViewModels;

namespace DentalApp.Presentation;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Configuração
		var configBuilder = new ConfigurationBuilder()
			.SetBasePath(FileSystem.AppDataDirectory)
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
		var configuration = configBuilder.Build();
		builder.Configuration.AddConfiguration(configuration);

		// Registro de dependências
		builder.Services.AddSingleton<IConfiguration>(configuration);
		builder.Services.AddSingleton<IApiKeyProvider, ApiKeyProvider>();
		builder.Services.AddHttpClient<IGeminiService, GeminiService>();
		builder.Services.AddScoped<IMarkdownExporter, ObsidianExporter>();
		builder.Services.AddScoped<IPatientRepository, PatientRepository>();
		builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
		builder.Services.AddScoped<IClinicalNoteRepository, ClinicalNoteRepository>();

		// DbContext (MySQL)
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		builder.Services.AddDbContext<AppDbContext>(options =>
			options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

		// ViewModels
		builder.Services.AddTransient<PatientsViewModel>();
		builder.Services.AddTransient<AppointmentsViewModel>();
		builder.Services.AddTransient<AIAssistantViewModel>();
		builder.Services.AddTransient<ObsidianExportViewModel>();

		return builder.Build();
	}
}