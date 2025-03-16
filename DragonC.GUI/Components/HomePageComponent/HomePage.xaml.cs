namespace DragonC.GUI.Components.HomePageComponent;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        BindingContext = new HomePageModel();
	}
}