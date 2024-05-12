using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CANSAT_UI.Services;
using System.IO;
using System.Reflection;
using System.Windows;
using Wpf.Ui;
using CANSAT_UI.Services.Contracts;
using System.IO.Ports;
using CANSAT_UI.ViewModels;
using CANSAT_UI.Repositories;

namespace CANSAT_UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static readonly IHost _host = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(c =>
        {
            c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!);
        })
        .ConfigureServices((context, services) =>
        {
            // App Host
            services.AddHostedService<ApplicationHostService>();

            // Page resolver service
            services.AddSingleton<IPageService, PageService>();

            // Serial communication service
            services.AddSingleton<ISerialCommunicationService, SerialCommunicationService>();
            services.AddSingleton<SerialPort>();

            // Service containing navigation, same as INavigationWindow... but without window
            services.AddSingleton<INavigationService, NavigationService>();

            // Inside your service registration code (e.g., ConfigureServices method in Startup.cs)
            services.AddSingleton<ILoggingService>(provider =>
            {
                // Get the base path configured in your application
                string basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;

                // Append a relative path to the log file name (e.g., "logs/myapp.log")
                string logFileName = "logs/app.log";
                string logFilePath = Path.Combine(basePath, logFileName);

                // Ensure that the directory exists before attempting to write the log file
                string logDirectory = Path.GetDirectoryName(logFilePath)!;
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Create an instance of FileLoggingService with the constructed log file path
                return new FileLoggingService(logFilePath);
            });

            // Main window container with navigation
            services.AddScoped<INavigationWindow, Views.Container>();
            services.AddScoped<ContainerViewModel>();

            // Views and ViewModels
            services.AddScoped<Views.Pages.Connection>();
            services.AddScoped<ConnectionViewModel>();

            services.AddScoped<Views.Pages.Dashboard>();
            services.AddScoped<DashboardViewModel>();

            services.AddScoped<Views.Pages.Database>();
            services.AddScoped<DatabaseViewModel>();

            // Repositories
            services.AddSingleton<IDataRepository, MySQLDataRepository>();
        })
        .Build();

    /// <summary>
    /// Gets registered service.
    /// </summary>
    /// <typeparam name="T">Type of the service to get.</typeparam>
    /// <returns>Instance of the service or <see langword="null"/>.</returns>
    public static T? GetService<T>() where T : class
    {
        return _host.Services.GetService(typeof(T)) as T;
    }

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        await _host.StartAsync();
    }

    /// <summary>
    /// Occurs when the application is closing.
    /// </summary>
    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();

        _host.Dispose();
    }
}
