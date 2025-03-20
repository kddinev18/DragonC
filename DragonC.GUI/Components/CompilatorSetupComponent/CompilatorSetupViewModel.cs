using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonC.Domain.Compilator;
using DragonC.Domain.Data;
using DragonC.Domain.Lexer.FormalGrammar;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.CommandsComponent;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.FormalGrammarEditorComponent.Models;
using DragonC.GUI.Components.CompilatorSetupComponent.Components.TokenSeparatorsComponent;
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
                            GenerateAllPossibleCharactersForRule(rule.Start,formalRules, variant.NonTerminalPart);
                            break;
                        case 2:
                            GnerateAllPossibleNumbersForRule(rule.Start, formalRules, variant.NonTerminalPart);
                            break;
                        case 3: 
                            break;
                        case 4: 
                            break;
                        case 5:
                            GenerateSpaceRule(rule.Start, formalRules, variant.NonTerminalPart);
                            break;
                        case 6: 
                            GenerateDynamiCommandRule(rule.Start, formalRules, variant.NonTerminalPart);
                            break;
                        case 7:
                            GenerateDynamicNameRule(rule.Start, formalRules, variant.NonTerminalPart);
                            break;
                        case 8:
                            GenerateDynamicValueRule(rule.Start, formalRules, variant.NonTerminalPart);
                            break;
                        case 9:
                            GenerateDynamicLiteralRule(rule.Start, formalRules, variant.NonTerminalPart);
                            break;
                        case 10:
                            GenerateCustomRule(rule.Start, formalRules, variant.TerminalPart, variant.NonTerminalPart);
                            break;
                        default: 
                            continue;
                    }
                }
            }

            DependencyService.Resolve<ICompilatorService>().CompilatorData = new CompilatorData()
            {
                BaseFormalRules = formalRules,
                TokenSeparators = separators,
                LowLevelCommands = commands
            };
        }

        private void GenerateCustomRule(string start, List<UnformatedRule> formalRules, string terminalPart, string nonTerminalPart)
        {
            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->{terminalPart}%{nonTerminalPart}%"
                });
            }
            else
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->{terminalPart}"
                });
            }
        }

        private void GenerateDynamicLiteralRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart)
        {
            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->&_&%{nonTerminalPart}%"
                });
            }
            else
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->&_&"
                });
            }
        }

        private void GenerateDynamicValueRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart)
        {
            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->@_@%{nonTerminalPart}%"
                });
            }
            else
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->@_@"
                });
            }
        }

        private void GenerateDynamicNameRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart)
        {
            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->|_|%{nonTerminalPart}%"
                });
            }
            else
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->|_|"
                });
            }
        }

        private void GenerateDynamiCommandRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart)
        {
            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->#_#%{nonTerminalPart}%"
                });
            }
            else
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}->#_#"
                });
            }
        }

        private void GenerateAllPossibleCharactersForRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart)
        {
            List<string> rules = new List<string>();

            for (char c = 'A'; c <= 'Z'; c++)
            {
                rules.Add($"{start}->{c}%{start}allCharacters%");
            }
            for (char c = 'a'; c <= 'z'; c++)
            {
                rules.Add($"{start}->{c}%{start}allCharacters%");
            }

            for (char c = 'A'; c <= 'Z'; c++)
            {
                rules.Add($"%{start}allCharacters%->{c}%{start}allCharacters%");
            }
            for (char c = 'a'; c <= 'z'; c++)
            {
                rules.Add($"%{start}allCharacters%->{c}%{start}allCharacters%");
            }

            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    rules.Add($"%{start}allCharacters%->{c}%{nonTerminalPart}%");
                }
                for (char c = 'a'; c <= 'z'; c++)
                {
                    rules.Add($"%{start}allCharacters%->{c}%{nonTerminalPart}%");
                }
            }
            else
            {
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    rules.Add($"%{start}allCharacters%->{c}");
                }
                for (char c = 'a'; c <= 'z'; c++)
                {
                    rules.Add($"%{start}allCharacters%->{c}");
                }
            }    


            formalRules.AddRange(rules.Select(x => new UnformatedRule()
            {
                Rule = x,
            }));
        }


        private void GnerateAllPossibleNumbersForRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart)
        {
            List<string> rules = new List<string>();

            for (int i = 0; i <= 9; i++)
            {
                rules.Add($"{start}->{i}%{start}allNumbers%");
            }

            for (int i = 0; i <= 9; i++)
            {
                rules.Add($"{start}allNumbers->{i}%{start}allNumbers%");
            }

            if (!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                for (int i = 0; i <= 9; i++)
                {
                    rules.Add($"{start}allNumbers->{i}%{nonTerminalPart}%");
                }
            }
            else
            {
                for (int i = 0; i <= 9; i++)
                {
                    rules.Add($"{start}allNumbers->{i}");
                }
            }

            formalRules.AddRange(rules.Select(x => new UnformatedRule()
            {
                Rule = x,
            }));
        }

        private void GenerateSpaceRule(string start, List<UnformatedRule> formalRules, string nonTerminalPart)
        {
            formalRules.Add(new UnformatedRule()
            {
                Rule = $"{start}-> %{start}space%"
            });

            if(!string.IsNullOrWhiteSpace(nonTerminalPart))
            {
                formalRules.Add(new UnformatedRule()
                {
                    Rule = $"{start}space->%{nonTerminalPart}%"
                });
            }
        }
    }
}