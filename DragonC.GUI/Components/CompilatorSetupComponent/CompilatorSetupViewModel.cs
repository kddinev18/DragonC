using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.Domain.Compilator;
using DragonC.Domain.Data;
using DragonC.Domain.Lexer.FormalGrammar;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.CommandsComponent;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Models;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.TokenSeparatorsComponent;
using DragonC.GUI.Services;
using DragonC.GUI.Services.Contracts;
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
            List<LowLevelCommand> commands = Commands.Commands.Select(x => new LowLevelCommand()
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
                    switch (variant.TerminalPartType.Id)
                    {
                        case 1:
                            GenerateAllPossibleCharactersForRule(rule.Start.NonTerminalSymbol,formalRules, variant.NonTerminalPart, variant.IsEntryRule);
                            break;
                        case 2:
                            GnerateAllPossibleNumbersForRule(rule.Start.NonTerminalSymbol, formalRules, variant.NonTerminalPart, variant.IsEntryRule);
                            break;
                        case 3: 
                            break;
                        case 4: 
                            break;
                        case 5:
                            GenerateSpaceRule(rule.Start.NonTerminalSymbol, formalRules, variant.NonTerminalPart, variant.IsEntryRule);
                            break;
                        case 6: 
                            GenerateDynamiCommandRule(rule.Start.NonTerminalSymbol, formalRules, variant.NonTerminalPart, variant.IsEntryRule);
                            break;
                        case 7:
                            GenerateDynamicNameRule(rule.Start.NonTerminalSymbol, formalRules, variant.NonTerminalPart, variant.IsEntryRule);
                            break;
                        case 8:
                            GenerateDynamicValueRule(rule.Start.NonTerminalSymbol, formalRules, variant.NonTerminalPart, variant.IsEntryRule);
                            break;
                        case 9:
                            GenerateDynamicLiteralRule(rule.Start.NonTerminalSymbol, formalRules, variant.NonTerminalPart, variant.IsEntryRule);
                            break;
                        case 10:
                            GenerateCustomRule(rule.Start.NonTerminalSymbol, formalRules, variant.TerminalPart, variant.NonTerminalPart, variant.IsEntryRule);
                            break;
                        default: 
                            continue;
                    }
                }
            }

            CompilatorService.Instance.CompilatorData = new CompilatorData()
            {
                BaseFormalRules = formalRules,
                TokenSeparators = separators,
                LowLevelCommands = commands
            };
        }

        private void GenerateCustomRule(string start, List<UnformatedRule> formalRules, string terminalPart, string nonTerminalPart, bool isStart)
        {
            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->{terminalPart}%{nonTerminalPart}%",
                    IsStart = isStart
                });
            }
            else
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->{terminalPart}",
                    IsStart = isStart
                });
            }
        }

        private void GenerateDynamicLiteralRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart, bool isStart)
        {
            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->&_&%{nonTerminalPart}%",
                    IsStart = isStart
                });
            }
            else
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->&_&",
                    IsStart = isStart
                });
            }
        }

        private void GenerateDynamicValueRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart, bool isStart)
        {
            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->@_@%{nonTerminalPart}%",
                    IsStart = isStart
                });
            }
            else
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->@_@",
                    IsStart = isStart
                });
            }
        }

        private void GenerateDynamicNameRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart , bool isStart)
        {
            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->|_|%{nonTerminalPart}%",
                    IsStart = isStart
                });
            }
            else
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->|_|",
                    IsStart = isStart
                });
            }
        }

        private void GenerateDynamiCommandRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart, bool isStart)
        {
            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->#_#%{nonTerminalPart}%",
                    IsStart = isStart
                });
            }
            else
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->#_#",
                    IsStart = isStart
                });
            }
        }

        private void GenerateAllPossibleCharactersForRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart, bool isStart)
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->{c}%{start}allCharacters%",
                    IsStart = isStart
                });
            }
            for (char c = 'a'; c <= 'z'; c++)
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->{c}%{start}allCharacters%",
                    IsStart = isStart
                });
            }

            for (char c = 'A'; c <= 'Z'; c++)
            {
                formalRules.Add(new UnformatedRule() { Rule = $"%{start}allCharacters%->{c}%{start}allCharacters%" });
            }
            for (char c = 'a'; c <= 'z'; c++)
            {
                formalRules.Add(new UnformatedRule() {  Rule = $"%{start}allCharacters%->{c}%{start}allCharacters%" });
            }

            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    formalRules.Add(new UnformatedRule() { Rule = $"%{start}allCharacters%->{c}%{nonTerminalPart}%", IsStart = isStart });
                }
                for (char c = 'a'; c <= 'z'; c++)
                {
                    formalRules.Add(new UnformatedRule() { Rule = $"%{start}allCharacters%->{c}%{nonTerminalPart}%", IsStart = isStart });
                }
            }
            else
            {
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    formalRules.Add(new UnformatedRule() { Rule = $"%{start}allCharacters%->{c}" });
                }
                for (char c = 'a'; c <= 'z'; c++)
                {
                    formalRules.Add(new UnformatedRule() { Rule = $"%{start}allCharacters%->{c}" });
                }
            }    
        }


        private void GnerateAllPossibleNumbersForRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart, bool isStart)
        {
            for (int i = 0; i <= 9; i++)
            {
                formalRules.Add(new UnformatedRule() { Rule = $"{start}->{i}%{start}allNumbers%", IsStart = isStart });
            }

            for (int i = 0; i <= 9; i++)
            {
                formalRules.Add(new UnformatedRule() { Rule = $"{start}allNumbers->{i}%{start}allNumbers%" });
            }

            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                for (int i = 0; i <= 9; i++)
                {
                    formalRules.Add(new UnformatedRule() { Rule = $"{start}allNumbers->{i}%{nonTerminalPart}%" });
                }
            }
            else
            {
                for (int i = 0; i <= 9; i++)
                {
                    formalRules.Add(new UnformatedRule() { Rule = $"{start}allNumbers->{i}" });
                }
            }
        }

        private void GenerateSpaceRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart, bool isStart)
        {
            if(!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}-> %{nonTerminalPart}%",
                    IsStart = isStart
                });
            }
            else
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}-> ",
                    IsStart = isStart
                });
            }
        }
    }
}