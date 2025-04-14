namespace DragonC.GUI.Components.LogInPageComponent;

public partial class LogInPage : ContentPage
{
	public LogInPage()
	{
		InitializeComponent();

		BindingContext = new LogInPageModel();
	}
}