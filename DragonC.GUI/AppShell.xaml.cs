using DragonC.GUI.Components.HomePageComponent;
using DragonC.GUI.Components.ProjectsPageComponent;

namespace DragonC.GUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("projects", typeof(ProjectsPage));
            Routing.RegisterRoute("home", typeof(HomePage));
        }
    }
}
