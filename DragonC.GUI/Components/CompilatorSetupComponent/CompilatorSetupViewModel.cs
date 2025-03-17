using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.CompilatorSetupComponent
{
    public partial class CompilatorSetupViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isExpanded = true;
    }
}
