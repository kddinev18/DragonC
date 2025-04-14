namespace DragonC.GUI.Components.ProjectsPageComponent;

public partial class ProjectsPage : ContentPage
{
    public ProjectsPage()
    {
        InitializeComponent();

        BindingContext = new ProjectsViewModel();
    }
}