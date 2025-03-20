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
                DisplayName = "DynamicCommand"
            },
            new NomenclatureModel()
            {
                Id = 7,
                DisplayName = "DynamicName"
            },
            new NomenclatureModel()
            {
                Id = 8,
                DisplayName = "DynamicValue"
            },
            new NomenclatureModel()
            {
                Id = 9,
                DisplayName = "DynamicLiteral"
            },
            new NomenclatureModel()
            {
                Id = 10,
                DisplayName = "Custom"
            },
        ];

        public ObservableCollection<FormalRuleModel> FormalRules { get; set; } = new ObservableCollection<FormalRuleModel>();

        public FormalGrammarEditorViewModel()
        {
            FormalRuleModel immValue = new FormalRuleModel();
            immValue.Start = "immValue";
            immValue.FormalRuleVariants.Add(new FormalRuleVariantModel(immValue)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 9).First(),
                IsEntryRule = true,
            });

            FormalRuleModel immValueConst = new FormalRuleModel();
            immValueConst.Start = "immValueConst";
            immValueConst.FormalRuleVariants.Add(new FormalRuleVariantModel(immValueConst)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 7).First(),
                IsEntryRule = true,
            });

            FormalRuleModel commandExec = new FormalRuleModel();
            commandExec.Start = "commandExec";
            commandExec.FormalRuleVariants.Add(new FormalRuleVariantModel(commandExec)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 6).First(),
                IsEntryRule = true,
            });




            FormalRuleModel constDecl = new FormalRuleModel();
            constDecl.Start = "constDecl";
            constDecl.FormalRuleVariants.Add(new FormalRuleVariantModel(constDecl)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 10).First(),
                TerminalPart = "const",
                NonTerminalPart = "space",
                IsEntryRule = true,
            });

            FormalRuleModel space = new FormalRuleModel();
            space.Start = "space";
            space.FormalRuleVariants.Add(new FormalRuleVariantModel(space)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 10).First(),
                TerminalPart = " ",
                NonTerminalPart = "constName",
                IsEntryRule = false,
            });

            FormalRuleModel constName = new FormalRuleModel();
            constName.Start = "constName";
            constName.FormalRuleVariants.Add(new FormalRuleVariantModel(constName)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 7).First(),
                NonTerminalPart = "2ndSpace",
                IsEntryRule = false,
            });

            FormalRuleModel ndSpace = new FormalRuleModel();
            ndSpace.Start = "2ndSpace";
            ndSpace.FormalRuleVariants.Add(new FormalRuleVariantModel(ndSpace)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 10).First(),
                TerminalPart = " ",
                NonTerminalPart = "constValue",
                IsEntryRule = false,
            });

            FormalRuleModel constValue = new FormalRuleModel();
            constValue.Start = "constValue";
            constValue.FormalRuleVariants.Add(new FormalRuleVariantModel(constValue)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 8).First(),
                IsEntryRule = false,
            });

            FormalRules.Add(immValue);
            FormalRules.Add(immValueConst);
            FormalRules.Add(commandExec);
            FormalRules.Add(constDecl);
            FormalRules.Add(space);
            FormalRules.Add(constName);
            FormalRules.Add(ndSpace);
            FormalRules.Add(constValue);
        }

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
