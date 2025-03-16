namespace DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent;

public partial class FormalGrammarEditorView : ContentView
{
	public FormalGrammarEditorView()
	{
		InitializeComponent();
		BindingContext = new FormalGrammarEditorViewModel();
	}
}