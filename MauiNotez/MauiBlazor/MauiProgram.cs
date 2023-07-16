using MauiBlazor.Data;
using Syncfusion.Blazor;

namespace MauiBlazor;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjU3NDM3MEAzMjMyMmUzMDJlMzBpWHZVbkZkZmpjV1prdzVndmdRaHBITFFNY0ZtVUE3WXpQU1JMNGRxVnUwPQ==");

		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>().ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });
		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddSyncfusionBlazor();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		builder.Services.AddSingleton<WeatherForecastService>();
		return builder.Build();
	}
}