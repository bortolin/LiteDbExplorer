using CommunityToolkit.Maui;
using LiteDbExplorer.Services;
using Microsoft.Extensions.Logging;

namespace LiteDbExplorer;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<ItemLookupPage>();
        builder.Services.AddTransient<ItemLookupViewModel>();
		builder.Services.AddSingleton<INavigationService, NavigationService>();
		builder.Services.AddSingleton<IFileService, FileService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}