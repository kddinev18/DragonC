using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Models;
using Syncfusion.Maui.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Models
{
    public partial class FormalRuleVariantModel : ObservableObject
    {
        private FormalRuleModel _parent;
        public FormalRuleVariantModel(FormalRuleModel parent)
        {
            _parent = parent;
        }
        [ObservableProperty]
        private string terminalPart;

        [ObservableProperty]
        private string nonTerminalPart;

        [ObservableProperty]
        private NomenclatureModel terminalPartType;

        [ObservableProperty]
        private bool customRuleInputVisible = false;

        [ObservableProperty]
        private bool isLast;

        [ObservableProperty]
        private GridLength gridLength = new GridLength(0);

        partial void OnTerminalPartTypeChanged(NomenclatureModel value)
        {
            CustomRuleInputVisible = value?.Id == 8;
            GridLength = value?.Id == 8 ? GridLength.Star : new GridLength(0);
        }

        [RelayCommand]
        private void AddVariant()
        {
            _parent.FormalRuleVariants.ForEach(x => x.IsLast = false);
            _parent.FormalRuleVariants.Add(new FormalRuleVariantModel(_parent)
            {
                IsLast = true
            });
        }

        [RelayCommand]
        private void DeleteVariant(FormalRuleVariantModel formalRule)
        {
            _parent.FormalRuleVariants.ForEach(x => x.IsLast = false);
            _parent.FormalRuleVariants.Remove(formalRule);
            if (_parent.FormalRuleVariants.Count == 0)
            {
                _parent.FormalRuleVariants.Add(new FormalRuleVariantModel(_parent)
                {
                    IsLast = true
                });
            }
            _parent.FormalRuleVariants.Last().IsLast = true;
        }
    }
}
