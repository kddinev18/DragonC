using CommunityToolkit.Maui;
using DragonC.GUI.Components.HomePageComponent;
using DragonC.GUI.Services;
using DragonC.GUI.Services.Contracts;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using Telerik.Maui.Controls.Compatibility;
using UraniumUI;

namespace DragonC.GUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseTelerik()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionCore()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MauiMaterialAssets.ttf", "MaterialAssets");
                    fonts.AddFontAwesomeIconFonts();
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
