using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent
{
    public partial class FormalGrammarEditorViewModel : ObservableObject
    {
        public ObservableCollection<FormalRuleModel> FormalRules { get; set; } = new ObservableCollection<FormalRuleModel>()
        {
            new FormalRuleModel()
        };

        [RelayCommand]
        private void Add(FormalRuleModel formalRule)
        {

        }

        [RelayCommand]
        private void Delete(FormalRuleModel formalRule)
        {

        }
    }
}
