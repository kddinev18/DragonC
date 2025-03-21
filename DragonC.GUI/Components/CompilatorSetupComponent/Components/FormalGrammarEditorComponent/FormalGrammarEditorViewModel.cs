using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Cmponents.NonTerminalsView;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Cmponents.NonTerminalsView.Models;
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
        [ObservableProperty]
        private ObservableCollection<NonTerminalModel> nonTerminalsSymbols = new ObservableCollection<NonTerminalModel>();
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
            NonTerminals = new NonTerminalsViewModel(this);

            FormalRuleModel immValue = new FormalRuleModel();
            immValue.Start = NonTerminals.NonTerminals.Where(x=>x.NonTerminalSymbol == "immValue").First();
            immValue.FormalRuleVariants.Add(new FormalRuleVariantModel(immValue)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 9).First(),
                IsEntryRule = true,
            });

            FormalRuleModel immValueConst = new FormalRuleModel();
            immValueConst.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "immValueConst").First();
            immValueConst.FormalRuleVariants.Add(new FormalRuleVariantModel(immValueConst)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 7).First(),
                IsEntryRule = true,
            });

            FormalRuleModel commandExec = new FormalRuleModel();
            commandExec.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "commandExec").First();
            commandExec.FormalRuleVariants.Add(new FormalRuleVariantModel(commandExec)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 6).First(),
                IsEntryRule = true,
            });




            FormalRuleModel constDecl = new FormalRuleModel();
            constDecl.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "constDecl").First();
            constDecl.FormalRuleVariants.Add(new FormalRuleVariantModel(constDecl)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 10).First(),
                TerminalPart = "const",
                NonTerminalPart = "space",
                IsEntryRule = true,
            });

            FormalRuleModel space = new FormalRuleModel();
            space.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "space").First();
            space.FormalRuleVariants.Add(new FormalRuleVariantModel(space)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 5).First(),
                NonTerminalPart = "constName",
                IsEntryRule = false,
            });

            FormalRuleModel constName = new FormalRuleModel();
            constName.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "constName").First();
            constName.FormalRuleVariants.Add(new FormalRuleVariantModel(constName)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 7).First(),
                NonTerminalPart = "2ndSpace",
                IsEntryRule = false,
            });

            FormalRuleModel ndSpace = new FormalRuleModel();
            ndSpace.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "2ndSpace").First();
            ndSpace.FormalRuleVariants.Add(new FormalRuleVariantModel(ndSpace)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 5).First(),
                NonTerminalPart = "constValue",
                IsEntryRule = false,
            });

            FormalRuleModel constValue = new FormalRuleModel();
            constValue.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "constValue").First();
            constValue.FormalRuleVariants.Add(new FormalRuleVariantModel(constValue)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 8).First(),
                IsEntryRule = false,
            });

            FormalRuleModel labelDecl = new FormalRuleModel();
            labelDecl.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "labelDecl").First();
            labelDecl.FormalRuleVariants.Add(new FormalRuleVariantModel(labelDecl)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 10).First(),
                TerminalPart = "label",
                NonTerminalPart = "spaceLbl",
                IsEntryRule = true,
            });

            FormalRuleModel spaceLbl = new FormalRuleModel();
            spaceLbl.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "spaceLbl").First();
            spaceLbl.FormalRuleVariants.Add(new FormalRuleVariantModel(spaceLbl)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 5).First(),
                NonTerminalPart = "labelName",
                IsEntryRule = false,
            });

            FormalRuleModel labelName = new FormalRuleModel();
            labelName.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "labelName").First();
            labelName.FormalRuleVariants.Add(new FormalRuleVariantModel(labelName)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 7).First(),
                IsEntryRule = false,
            });

            FormalRuleModel condCommandExec = new FormalRuleModel();
            condCommandExec.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "condCommandExec").First();
            condCommandExec.FormalRuleVariants.Add(new FormalRuleVariantModel(condCommandExec)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 6).First(),
                NonTerminalPart = "spaceCmd",
                IsEntryRule = true,
            });

            FormalRuleModel spaceCmd = new FormalRuleModel();
            spaceCmd.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "spaceCmd").First();
            spaceCmd.FormalRuleVariants.Add(new FormalRuleVariantModel(spaceCmd)
            {
                IsLast = true,
                TerminalPartType = TerminalPartTypes.Where(x => x.Id == 5).First(),
                NonTerminalPart = "condCommandParam",
                IsEntryRule = false,
            });

            FormalRuleModel condCommandParam = new FormalRuleModel();
            condCommandParam.Start = NonTerminals.NonTerminals.Where(x => x.NonTerminalSymbol == "condCommandParam").First();
            condCommandParam.FormalRuleVariants.Add(new FormalRuleVariantModel(condCommandParam)
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
            FormalRules.Add(labelDecl);
            FormalRules.Add(spaceLbl);
            FormalRules.Add(labelName);
            FormalRules.Add(condCommandExec);
            FormalRules.Add(spaceCmd);
            FormalRules.Add(condCommandParam);
        }

        public NonTerminalsViewModel NonTerminals { get; set; }

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

        public void UpdateCollection(List<NonTerminalModel> terminals)
        {
            NonTerminalsSymbols = new ObservableCollection<NonTerminalModel>(terminals);
        }

        public void AddItem(NonTerminalModel item)
        {
            NonTerminalsSymbols.Add(item);
        }

        public void RemoveItem(NonTerminalModel item)
        {
            NonTerminalsSymbols.Remove(item);
        }
    }
}
