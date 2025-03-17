using Microsoft.Extensions.Configuration;

namespace DragonC.GUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            Application.Current.UserAppTheme = AppTheme.Light;
        }
    }
}
