using System.ComponentModel;

namespace DragonC.GUI.Components.CodeEditorComponent;

public partial class CodeEditorView : ContentView
{
    public CodeEditorView()
    {
        InitializeComponent();
        BindingContext = new CodeEditorViewModel();
    }
}