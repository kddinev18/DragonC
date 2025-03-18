using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Cmponents.NonTerminalsView.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Cmponents.NonTerminalsView
{
    public partial class NonTerminalsViewModel : ObservableObject
    {
        public ObservableCollection<NonTerminalModel> Terminals { get; set; } = [

            new NonTerminalModel()
            {
                TerminalSymbol = "S"
            },
            new NonTerminalModel()
            {
                TerminalSymbol = "N"
            }
        ];

        [RelayCommand]
        private void Add()
        {
            Terminals.Add(new NonTerminalModel()
            {
                TerminalSymbol = ""
            });
        }

        [RelayCommand]
        private void Delete(NonTerminalModel terminal)
        {
            Terminals.Remove(terminal);
        }
    }
}
