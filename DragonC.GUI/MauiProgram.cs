using CommunityToolkit.Maui;
using DragonC.GUI.Components.HomePageComponent;
using DragonC.GUI.Services;
using DragonC.GUI.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Syncfusion.Maui.Core.Hosting;
using UraniumUI;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

namespace DragonC.GUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
#if WINDOWS
                .ConfigureLifecycleEvents(events =>
                {
                    events.AddWindows(wndLifeCycleBuilder =>
                    {
                        wndLifeCycleBuilder.OnWindowCreated(window =>
                        {
                            IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                            AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);
                            if(winuiAppWindow.Presenter is OverlappedPresenter p)
                                p.Maximize();
                            else
                            {
                                const int width = 1200;
                                const int height = 800;
                                winuiAppWindow.MoveAndResize(new RectInt32(1920 / 2 - width / 2, 1080 / 2 - height / 2, width, height));
                            }
                        });
                    });
                })
#endif
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
