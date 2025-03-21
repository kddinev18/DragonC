using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Cmponents.NonTerminalsView.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Cmponents.NonTerminalsView
{
    public partial class NonTerminalsViewModel : ObservableObject
    {
        private FormalGrammarEditorViewModel _parent;
        public ObservableCollection<NonTerminalModel> NonTerminals { get; set; }

        public NonTerminalsViewModel(FormalGrammarEditorViewModel parent)
        {
            _parent = parent;

            NonTerminals = [
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "immValue"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "immValueConst"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "commandExec"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "constDecl"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "space"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "constName"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "2ndSpace"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "constValue"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "labelDecl"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "spaceLbl"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "labelName"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "condCommandExec"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "spaceCmd"
                },
                new NonTerminalModel(this)
                {
                    NonTerminalSymbol = "condCommandParam"
                },
            ];
            UpdateCollection();
        }

        [RelayCommand]
        private void Add()
        {
            NonTerminalModel terminalModel = new NonTerminalModel(this)
            {
                NonTerminalSymbol = ""
            };
            _parent.AddItem(terminalModel);
            NonTerminals.Add(terminalModel);
        }

        [RelayCommand]
        private void Delete(NonTerminalModel terminal)
        {
            _parent.RemoveItem(terminal);
            NonTerminals.Remove(terminal);
        }

        public void UpdateCollection()
        {
            if(NonTerminals != null)
            {
                _parent.UpdateCollection(NonTerminals.ToList());
            }
        }
    }
}
