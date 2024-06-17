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
            services.AddSingleton<MySQLDataRepository>();
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
