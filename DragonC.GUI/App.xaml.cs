using Microsoft.Extensions.Configuration;

namespace DragonC.GUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXxfd3VVQ2VeV0F3W0c=");
            MainPage = new AppShell();
        }
    }
}
