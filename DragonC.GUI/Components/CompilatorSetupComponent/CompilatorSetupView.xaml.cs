using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DragonC.GUI.Components.CompilatorSetupComponent;

public partial class CompilatorSetupView : ContentView
{
    public CompilatorSetupView()
    {
        InitializeComponent();
        BindingContext = new CompilatorSetupViewModel();
    }
}