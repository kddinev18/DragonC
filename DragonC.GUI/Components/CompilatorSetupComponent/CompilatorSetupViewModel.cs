using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer.FormalGrammar;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.CommandsComponent;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Models;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.TokenSeparatorsComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.CompilatorSetupComponent
{
    public partial class CompilatorSetupViewModel : ObservableObject
    {
        public CommandsViewModel Commands { get; set; } = new CommandsViewModel();
        public TokensViewModel Separators { get; set; } = new TokensViewModel();
        public FormalGrammarEditorViewModel FormalGrammar { get; set; } = new FormalGrammarEditorViewModel();

        [RelayCommand]
        private void Save()
        {
            List<LowLevelCommand> commands = Commands.Commands.Select(x=> new LowLevelCommand()
            {
                CommandName = x.Name,
                MachineCode = x.MachineCode,
                IsConditionalCommand = x.IsConditional
            }).ToList();

            List<string> separators = Separators.Tokens.Select(x => x.Separator).ToList();

            List<UnformatedRule> formalRules = new List<UnformatedRule>();
            foreach (FormalRuleModel rule in FormalGrammar.FormalRules)
            {
                foreach (FormalRuleVariantModel variant in rule.FormalRuleVariants)
                {
                    formalRules.Add(new UnformatedRule()
                    {
                        IsStart = variant.IsEntryRule,
                        Rule = 
                    });
                }
            }
        }
    }
}
