using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Cmponents.NonTerminalsView;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Models;
using DragonC.GUI.Models;
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
        public List<NomenclatureModel> TerminalPartTypes { get; set; } =
        [
            new NomenclatureModel()
            {
                Id = 1,
                DisplayName = "Characters"
            },
            new NomenclatureModel()
            {
                Id = 2,
                DisplayName = "Numbers"
            },
            new NomenclatureModel()
            {
                Id = 3,
                DisplayName = "Special characters"
            },
            new NomenclatureModel()
            {
                Id = 4,
                DisplayName = "Anything"
            },
            new NomenclatureModel()
            {
                Id = 5,
                DisplayName = "Space"
            },
            new NomenclatureModel()
            {
                Id = 6,
                DisplayName = "Command"
            },
            new NomenclatureModel()
            {
                Id = 7,
                DisplayName = "DynamicName"
            },
            new NomenclatureModel()
            {
                Id = 8,
                DisplayName = "Custom"
            },
        ];

        public ObservableCollection<FormalRuleModel> FormalRules { get; set; } = new ObservableCollection<FormalRuleModel>()
        {
            new FormalRuleModel()
        };

        public NonTerminalsViewModel NonTerminals { get; set; } = new NonTerminalsViewModel();

        [RelayCommand]
        private void Add()
        {
            FormalRules.Add(new FormalRuleModel());
        }

        [RelayCommand]
        private void Delete(FormalRuleModel formalRule)
        {
            FormalRules.Remove(formalRule);
        }
    }
}
