using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using System.Globalization;

namespace skps_services
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en");

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}