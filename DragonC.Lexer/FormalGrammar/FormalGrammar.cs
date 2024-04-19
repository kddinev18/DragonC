using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace DragonC.Lexer.FormalGrammar
{
    public class FormalGrammar
    {
        private List<string> _terminalSybols = new List<string>();
        private List<string> _nonTerminalSymbols = new List<string>();
        private List<FormalGrammarRule> _formalGrammarRules = new List<FormalGrammarRule>();
        private List<string> _labels = new List<string>();
        public string NonTerminalIndicator { get; set; } = "%";
        public List<Command> Commands { get; set; }

        public FormalGrammar(List<Command> commands)
        {
            Commands = commands;
        }

        public void SetRules(List<UnformatedRule> unformatedRules)
        {
            foreach (UnformatedRule unformatedRule in unformatedRules)
            {
                AddRule(unformatedRule);
            }
            LinkRules();

            _nonTerminalSymbols = _formalGrammarRules
                .SelectMany(x => x.PossibleOutcomes)
                .Select(x => x.NonTerminalPart)
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .ToList();
            _terminalSybols = _formalGrammarRules
                .SelectMany(x => x.PossibleOutcomes)
                .Select(x => x.TerminalPart)
                .Distinct()
                .ToList();
        }

        private void AddRule(UnformatedRule unformatedRule)
        {
            RuleComponents ruleComponents = GetRuleComponents(unformatedRule);
            bool contains = false;
            foreach (FormalGrammarRule rule in _formalGrammarRules)
            {
                if (rule.StartNonTerminalSymbol == ruleComponents.StartSymvol)
                {
                    rule.PossibleOutcomes.Add(new Rule()
                    {
                        TerminalPart = ruleComponents.TerminalPart,
                        NonTerminalPart = ruleComponents.NonTerminalPart,
                    });
                    contains = true;
                }
            }

            if (!contains)
            {

                _formalGrammarRules.Add(new FormalGrammarRule()
                {
                    StartNonTerminalSymbol = ruleComponents.StartSymvol,
                    IsStart = ruleComponents.IsStart,
                    PossibleOutcomes = new List<Rule>() {
                        new Rule()
                        {
                            TerminalPart = ruleComponents.TerminalPart,
                            NonTerminalPart = ruleComponents.NonTerminalPart,
                        }
                    }
                });
            }
        }

        private RuleComponents GetRuleComponents(UnformatedRule unformatedRule)
        {
            string[] ruleComponents = unformatedRule.Rule.Split("->");
            bool isFinal = ruleComponents[1].Split(NonTerminalIndicator).Count() == 1;
            return new RuleComponents()
            {
                StartSymvol = ruleComponents[0],
                TerminalPart = ruleComponents[1].Split(NonTerminalIndicator)[0],
                NonTerminalPart = isFinal ? "" : ruleComponents[1].Split(NonTerminalIndicator)[1],
                IsStart = unformatedRule.IsStart,
            };
        }

        public void LinkRules()
        {
            for (int i = 0; i < _formalGrammarRules.Count(); i++)
            {
                for (int j = 0; j < _formalGrammarRules[i].PossibleOutcomes.Count(); j++)
                {
                    for (int k = 0; k < _formalGrammarRules.Count(); k++)
                    {
                        if (_formalGrammarRules[i].PossibleOutcomes[j].NonTerminalPart == _formalGrammarRules[k].StartNonTerminalSymbol)
                        {
                            _formalGrammarRules[i].PossibleOutcomes[j].Next = _formalGrammarRules[k];
                        }
                    }
                }
            }
        }

        public List<TokenUnit> EvaluateTokens(List<TokenUnit> tokens)
        {
            LoadLabels(tokens);
            for (int i = 0; i < tokens.Count(); i++)
            {
                tokens[i] = EvaluateToken(tokens[i]);
            }

            return tokens;
        }

        private void LoadLabels(List<TokenUnit> tokens)
        {
            foreach(TokenUnit token in tokens)
            {
                string[] splits = token.Token.Split(' ');
                if (splits[0] == "label")
                {
                    _labels.Add(splits[1]);
                }
            }
        }

        private TokenUnit EvaluateToken(TokenUnit tokenUnit)
        {
            tokenUnit = ReplceDyamicValues(tokenUnit);
            if(!tokenUnit.IsValid)
            {
                return tokenUnit;
            }
            string token = tokenUnit.Token;
            foreach (var startFormalRule in _formalGrammarRules.Where(x => x.IsStart))
            {
                int possibelOutcomeIndex = 0;
                FormalGrammarRule currentFormlRule = startFormalRule;
                Rule rule = currentFormlRule.PossibleOutcomes[0];
                while (true)
                {
                    if (currentFormlRule.PossibleOutcomes.Count() == possibelOutcomeIndex - 1)
                    {
                        return GetError(token, tokenUnit);
                    }
                    if (rule.Next == null)
                    {
                        if (rule.TerminalPart == token)
                        {
                            return tokenUnit;
                        }
                        else
                        {
                            for (int i = 0; i < currentFormlRule.PossibleOutcomes.Count(); i++)
                            {
                                if (currentFormlRule.PossibleOutcomes[i].Next != null)
                                {
                                    rule = currentFormlRule.PossibleOutcomes[i];
                                }
                            }
                            if (rule.Next == null)
                            {
                                return tokenUnit;
                            }
                        }
                    }

                    try
                    {
                        if (rule.TerminalPart == token.Substring(0, rule.TerminalPart.Length))
                        {
                            string newWord = token.Substring(rule.TerminalPart.Length, token.Length - rule.TerminalPart.Length);
                            if (string.IsNullOrEmpty(newWord))
                            {
                                if (rule.IsFinal)
                                {
                                    return tokenUnit;
                                }
                                return GetError(token, tokenUnit);
                            }

                            currentFormlRule = rule.Next;
                            possibelOutcomeIndex = 0;
                            rule = currentFormlRule.PossibleOutcomes[possibelOutcomeIndex++];
                            token = newWord;
                        }
                        else
                        {
                            if (currentFormlRule.PossibleOutcomes.Count() == possibelOutcomeIndex)
                            {
                                break;
                            }
                            rule = currentFormlRule.PossibleOutcomes[possibelOutcomeIndex++];
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return GetError(token, tokenUnit);
                    }
                }
            }
            return GetError(token, tokenUnit);
        }

        private TokenUnit ReplceDyamicValues(TokenUnit token)
        {
            string[] tokens = token.Token.Split(' ');

            if(tokens.Length == 1)
            {
                if (IsCommand(tokens[0]))
                {
                    tokens[0] = "|dynamicCommandName|";
                }
                else
                {
                    token = GetError(token.Token, token);
                }
            }
            else if (tokens.Length == 2)
            {
                if (IsConditionalCommand(tokens[0]) && IsLiteralOrLabel(tokens[1]))
                {
                    tokens[1] = "|dynamicCondCommandParam|";
                }
                else if(tokens[0] == "label" && !IsDynamicValueKeyWord(tokens[1]))
                {
                    tokens[1] = "|dynamicLabelName|";
                }
                else
                {
                    token = GetError(token.Token, token);
                }
            }
            else if (tokens.Length == 3)
            {
                if (tokens[0] == "const" && !IsDynamicValueKeyWord(tokens[1]) && IsLiteral(tokens[2]))
                {
                    tokens[1] = "|dynamicConstName|";
                    tokens[2] = "|dynamicConstValue|";
                }
                else
                {
                    token = GetError(token.Token, token);
                }
            }
            token.Token = string.Join(' ', tokens);

            return token;
        }

        private bool IsConditionalCommand(string token)
        {
            return Commands.Where(x => x.IsConditionalCommand).Select(x => x.CommandName).Contains(token);
        }

        private bool IsCommand(string token)
        {
            return Commands.Where(x => !x.IsConditionalCommand).Select(x => x.CommandName).Contains(token);
        }

        private bool IsLiteral(string token)
        {
            return int.TryParse(token, out var value);
        }

        private bool IsLiteralOrLabel(string token)
        {
            return int.TryParse(token, out var value) || _labels.Contains(token);
        }

        private bool IsDynamicValueKeyWord(string token)
        {
            return _terminalSybols.Contains(token);
        }

        private TokenUnit GetError(string token, TokenUnit tokenUnit)
        {
            tokenUnit.IsValid = false;
            tokenUnit.ErrorMessaes = new List<string>() { $"{token.Split(' ')[0]} is not a recognised command or keyword" };
            tokenUnit.StartCharacterOfErrorPosition = tokenUnit.Token.IndexOf(token + 1);

            return tokenUnit;
        }
    }
}
