using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.Domain.Compilator;
using DragonC.GUI.Services;
using DragonC.GUI.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DragonC.GUI.Components.CodeEditorComponent
{
    public partial class CodeEditorViewModel : ObservableObject
    {
        [ObservableProperty]
        private string code;

        [ObservableProperty]
        private string intermidiate;

        [ObservableProperty]
        private string output;

        [RelayCommand]
        private void RunCode()
        {
            CompilatorService compilator = CompilatorService.Instance;
            if (compilator.CompilatorData != null)
            {
                CompiledCode compiledCode = compilator.Compile(code.Trim());

                if(compiledCode.IsBuildSuccessfully)
                {
                    Intermidiate = string.Join('\r', compiledCode.InterMediateCommands);
                    Output = string.Join('\r', compiledCode.CompiledCommands);
                }
                else
                {
                    Output = string.Join('\r', compiledCode.ErrorMessages);
                }
            }
        }

        [RelayCommand]
        private void Reset()
        {

        }
    }
}
