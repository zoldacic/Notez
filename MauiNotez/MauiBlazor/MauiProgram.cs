using MauiBlazor.Data;
using Syncfusion.Blazor;
using System.Globalization;

namespace MauiBlazor;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWH1ceXVQQmJdV0VxXEE=");

		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>().ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });
		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddSyncfusionBlazor();


#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		builder.Services.AddSingleton<WeatherForecastService>();

        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("sv-SE");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("sv-SE");

        return builder.Build();
	}
}