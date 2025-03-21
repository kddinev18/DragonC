using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Cmponents.NonTerminalsView.Models
{
    public partial class NonTerminalModel : ObservableObject
    {
        private NonTerminalsViewModel _parent;
        [ObservableProperty]
        private string nonTerminalSymbol;

        partial void OnNonTerminalSymbolChanged(string value)
        {
            _parent.UpdateCollection();
        }
        public NonTerminalModel(NonTerminalsViewModel parent)
        {
            _parent = parent;
        }
    }
}
