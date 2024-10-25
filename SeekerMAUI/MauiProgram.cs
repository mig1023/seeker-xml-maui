using Microsoft.Extensions.Logging;

namespace SeekerMAUI
{
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
                    fonts.AddFont("YanoneKaffeesatz-Light.ttf", "YanoneFont");
                    fonts.AddFont("YanoneKaffeesatz-Bold.ttf", "YanoneFontBold");
                    fonts.AddFont("YanoneKaffeesatz-Italic.ttf", "YanoneFontItalic");
                    fonts.AddFont("Roboto-Thin.ttf", "RobotoFontThin");
                    fonts.AddFont("Roboto-Light.ttf", "RobotoFont");
                    fonts.AddFont("Roboto-Italic.ttf", "RobotoFontItalic");
                    fonts.AddFont("Roboto-Bold.ttf", "RobotoFontBold");
                    fonts.AddFont("CourierNew.ttf", "CourierFont");
                    fonts.AddFont("Daray.ttf", "Daray");
                    fonts.AddFont("Daray.ttf", "DarayBold");
                    fonts.AddFont("St.Sign.ttf", "St.Sign");
                    fonts.AddFont("St.Sign.ttf", "St.SignBold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
