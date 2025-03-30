namespace DragonC.GUI.Components.HighLevelCommandsComponent;

public partial class HighLevelComandView : ContentView
{
	public HighLevelComandView()
	{
		InitializeComponent();

		BindingContext = new HighLevelCommandsViewModel();
	}
}