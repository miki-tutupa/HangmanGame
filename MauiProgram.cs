using Microsoft.Extensions.Logging;

namespace Hangman;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("pincelhandwrite.ttf", "Handwrite");
                fonts.AddFont("icons.ttf", "Icons");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
