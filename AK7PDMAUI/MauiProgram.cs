using AK7PDMAUI.Services;
using AK7PDMAUI.ViewModels;
using Microsoft.Extensions.Logging;

namespace AK7PDMAUI;

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
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<CatalogPage>();
		builder.Services.AddSingleton<CreateBookPage>();
		builder.Services.AddSingleton<EditBookPage>();

		builder.Services.AddSingleton<RepositoryService>();

		builder.Services.AddSingleton<CatalogPageViewModel>();
		builder.Services.AddSingleton<LoginPageViewModel>();
		builder.Services.AddSingleton<CreateBookPageViewModel>();
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<EditBookPageViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	public static void ChangeWindowSize(int width, int height)
	{
		App.Current.Windows.First().Height = height;
		App.Current.Windows.First().Width = width;
	}
}
