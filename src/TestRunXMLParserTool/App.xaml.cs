using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using TestRunXMLParserTool.Models;
using TestRunXMLParserTool.Services;
using TestRunXMLParserTool.ViewModels;
using TestRunXMLParserTool.Views;

namespace TestRunXMLParserTool;

public partial class App : Application
{
	public static IHost? AppHost { get; private set; }
	App()
	{
		setCurrentCulture();

		AppHost = Host.CreateDefaultBuilder()
			.ConfigureServices(services =>
			{
				services.AddSingleton<MainWindowView>();
				services.AddSingleton<MainWindowViewModel>();
				services.AddSingleton<SettingsViewModel>();
				services.AddWindowFactory<SettingsWindowView>();
			})
			.Build();
		
	}

	protected override async void OnStartup(StartupEventArgs e)
	{
		await AppHost!.StartAsync();			

		var mainWindow = AppHost.Services.GetRequiredService<MainWindowView>();
		mainWindow.Show();

		base.OnStartup(e);
	}

	protected override async void OnExit(ExitEventArgs e)
	{
		await AppHost!.StopAsync();
		base.OnExit(e);
	}

	private void setCurrentCulture()
	{
		var culture = AppConfiguration.GetCulture();

		if (culture != null)
		{
			System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
		}
	}
}
