using CommunityToolkit.Maui;
using downpour.Data;
using downpour.Popups;
using downpour.ViewModels;
using Microsoft.Extensions.Logging;

namespace downpour;

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
				fonts.AddFont("Liteon.otf", "LiteOn");
				fonts.AddFont("Liteon-Black.otf", "LiteOnBlack");
				fonts.AddFont("Liteon-Bold.otf", "LiteOnBold");
				fonts.AddFont("Liteon-Light.otf", "LiteOnLight");
				fonts.AddFont("Liteon-Medium.otf", "LiteOnMedium");
				fonts.AddFont("Liteon-Thin.otf", "LiteOnThin");
			});
        builder.Services.AddSingleton<database>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
