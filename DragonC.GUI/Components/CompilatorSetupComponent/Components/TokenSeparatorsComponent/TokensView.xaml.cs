namespace DragonC.GUI.Components.CompilatorSetupComponent.Components.TokenSeparatorsComponent;

public partial class TokensView : ContentView
{
	public TokensView()
	{
		InitializeComponent();
		BindingContext = new TokensViewModel();
	}
}