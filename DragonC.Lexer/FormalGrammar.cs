using DragonC.Domain.Compilator;
using DragonC.Domain.Exceptions;
using DragonC.Domain.Lexer.FormalGrammar;
using DragonC.Domain.Lexer.Tokeniser;

namespace DragonC.Lexer
{
    public class FormalGrammar
    {
        private List<string> _terminalSymbols = new List<string>();
        private List<string> _dyanicTerminalSymblos = new List<string>();
        private List<string> _staticTerminalSymblos = new List<string>();
        private List<string> _nonTerminalSymbols = new List<string>();
        private List<FormalGrammarRule> _formalGrammarRules = new List<FormalGrammarRule>();
        private List<string> _labels = new List<string>();
        private List<string> _consts = new List<string>();

        public List<string> AllowedValuesForHighLevelCommands { get; set; } = new List<string>();
        public bool AllowLiteralsForHighLevelCommands { get; set; } = false;
        public int NumberOfArguments { get; set; } = 0;

        public string NonTerminalIndicator { get; set; } = "%";
        public string DynamicNamesIndicator { get; set; } = "|";
        public string DynamicValuesIndicator { get; set; } = "@";
        public string DynamicLiteralIndicator { get; set; } = "&";
        public string DynamicCommandIndicator { get; set; } = "#";

        private List<string> _dynamicIndicators = new List<string>();

        public List<LowLevelCommand> Commands { get; set; }

        public FormalGrammar(List<LowLevelCommand> commands)
        {
            Commands = commands;
            _dynamicIndicators.Add(DynamicNamesIndicator);
            _dynamicIndicators.Add(DynamicValuesIndicator);
            _dynamicIndicators.Add(DynamicCommandIndicator);
            _dynamicIndicators.Add(DynamicLiteralIndicator);
        }

        public FormalGrammar()
        {
            Commands = new List<LowLevelCommand>();
            _dynamicIndicators.Add(DynamicNamesIndicator);
            _dynamicIndicators.Add(DynamicValuesIndicator);
            _dynamicIndicators.Add(DynamicCommandIndicator);
            _dynamicIndicators.Add(DynamicLiteralIndicator);
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
            _terminalSymbols = _formalGrammarRules
                .SelectMany(x => x.PossibleOutcomes)
                .Select(x => x.TerminalPart)
                .Distinct()
                .ToList();

            foreach (string indicator in _dynamicIndicators)
            {
                _dyanicTerminalSymblos.AddRange(_terminalSymbols
                    .Where(x => x.StartsWith(indicator) && x.EndsWith(indicator))
                );
                _staticTerminalSymblos.AddRange(_terminalSymbols
                    .Where(x => !x.StartsWith(indicator) && !x.EndsWith(indicator))
                );
            }
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
            try
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
            catch (Exception)
            {
                throw new IvalidFormalRuleExcepetion();
            }
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

        public List<TokenUnit> EvaluateTokens(List<TokenUnit> tokens, bool isHighLevelCommand = false)
        {
            LoadLabels(tokens);
            LoadConsts(tokens);
            for (int i = 0; i < tokens.Count(); i++)
            {
                tokens[i] = EvaluateToken(tokens[i], isHighLevelCommand);
            }

            return tokens;
        }

        private void LoadConsts(List<TokenUnit> tokens)
        {
            foreach (TokenUnit token in tokens)
            {
                string[] splits = token.Token.Split(' ');
                if (splits[0] == "const")
                {
                    _consts.Add(splits[1]);
                }
            }
        }

        private void LoadLabels(List<TokenUnit> tokens)
        {
            foreach (TokenUnit token in tokens)
            {
                string[] splits = token.Token.Split(' ');
                if (splits[0] == "label")
                {
                    _labels.Add(splits[1]);
                }
            }
        }

        private TokenUnit EvaluateToken(TokenUnit tokenUnit, bool isHighLevelCommand = false)
        {
            tokenUnit = ReplceDyamicValues(tokenUnit, isHighLevelCommand);
            if (!tokenUnit.IsValid)
            {
                return tokenUnit;
            }
            string token = "";
            foreach (var startFormalRule in _formalGrammarRules.Where(x => x.IsStart))
            {
                token = tokenUnit.Token;
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
                        if (_dyanicTerminalSymblos.Contains(rule.TerminalPart))
                        {
                            bool flag = false;
                            foreach (string separator in _dynamicIndicators)
                            {
                                if (token.StartsWith(separator) && token.Split(separator).Count() == 3)
                                {
                                    token = ValidateDynamicValue(token, separator);

                                    if (string.IsNullOrEmpty(token))
                                    {
                                        return tokenUnit;
                                    }
                                    else
                                    {
                                        flag = true;
                                    }
                                }
                            }
                            if (flag)
                            {
                                break;
                            }
                        }
                        else if (rule.TerminalPart == token)
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
                        if (_dyanicTerminalSymblos.Contains(rule.TerminalPart))
                        {
                            bool skip = false;
                            foreach (string separator in _dynamicIndicators)
                            {
                                string[] splts = token.Split(separator);
                                if (token.StartsWith(separator) &&
                                    (isHighLevelCommand ? CheckForNUmberOfArgumets(NumberOfArguments) == splts.Length : splts.Length == 3))
                                {
                                    token = ValidateDynamicValue(token, separator);

                                    currentFormlRule = rule.Next;
                                    possibelOutcomeIndex = 0;
                                    rule = currentFormlRule.PossibleOutcomes[possibelOutcomeIndex++];
                                    skip = true;
                                    break;
                                }
                            }
                            if (!skip)
                            {
                                if (currentFormlRule.PossibleOutcomes.Count() == possibelOutcomeIndex)
                                {
                                    break;
                                }
                                rule = currentFormlRule.PossibleOutcomes[possibelOutcomeIndex++];
                            }
                        }
                        else if (rule.TerminalPart == token.Substring(0, rule.TerminalPart.Length))
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
        public static int CheckForNUmberOfArgumets(int numberOfArguments)
        {
            return 2 * numberOfArguments + 1;
        }

        private string ValidateDynamicValue(string token, string separator)
        {
            token = token.Substring(1, token.Length - 1);
            int indexOfSeparator = token.IndexOf(separator) + 1;
            token = token.Substring(indexOfSeparator, token.Length - indexOfSeparator);
            return token;
        }

        private TokenUnit ReplceDyamicValues(TokenUnit token, bool isHighLevelCommand = false)
        {
            if (isHighLevelCommand)
            {
                return ReplaceDynamicValuesForHighLevelCommands(token);
            }
            else
            {
                return ReplaveDyamicValuesForLowLevelCommands(token);
            }
        }

        private TokenUnit ReplaceDynamicValuesForHighLevelCommands(TokenUnit token)
        {
            string[] tokens = token.Token.Split(' ');

            for (int i = 0; i < tokens.Length; i++)
            {
                if (EvaluateAllowedValues(tokens[i]))
                {
                    string separator = AllowLiteralsForHighLevelCommands ? DynamicValuesIndicator : DynamicNamesIndicator;
                    tokens[i] = $"{separator}{tokens[i]}{separator}";
                }
            }
            token.Token = string.Join(' ', tokens);
            return token;
        }

        private TokenUnit ReplaveDyamicValuesForLowLevelCommands(TokenUnit token)
        {
            string[] tokens = token.Token.Split(' ');

            if (tokens.Length == 1)
            {
                if (IsCommand(tokens[0]))
                {
                    tokens[0] = $"{DynamicCommandIndicator}{tokens[0]}{DynamicCommandIndicator}";
                }
                else if (IsConstName(tokens[0]))
                {
                    tokens[0] = $"{DynamicNamesIndicator}{tokens[0]}{DynamicNamesIndicator}";
                }
                else if (IsLiteral(tokens[0]))
                {
                    tokens[0] = $"{DynamicLiteralIndicator}{tokens[0]}{DynamicLiteralIndicator}";
                }
                else
                {
                    token = GetError(tokens[0], token);
                }
            }
            else if (tokens.Length == 2)
            {
                if (IsConditionalCommand(tokens[0]) && IsLiteralOrLabel(tokens[1]) || IsConstName(tokens[1]))
                {
                    tokens[0] = $"{DynamicCommandIndicator}{tokens[0]}{DynamicCommandIndicator}";
                    tokens[1] = $"{DynamicValuesIndicator}{tokens[1]}{DynamicValuesIndicator}";
                }
                else if (tokens[0] == "label" && !IsDynamicValueKeyWord(tokens[1]))
                {
                    tokens[1] = $"{DynamicNamesIndicator}{tokens[1]}{DynamicNamesIndicator}";
                }
                else
                {
                    token = GetError(tokens[1], token);
                }
            }
            else if (tokens.Length == 3)
            {
                if (tokens[0] == "const" && !IsDynamicValueKeyWord(tokens[1]) && IsLiteral(tokens[2]))
                {
                    tokens[1] = $"{DynamicNamesIndicator}{tokens[1]}{DynamicNamesIndicator}";
                    tokens[2] = $"{DynamicValuesIndicator}{tokens[2]}{DynamicValuesIndicator}";
                }
                else
                {
                    token = GetError(string.Join(" ", tokens), token);
                }
            }
            token.Token = string.Join(' ', tokens);
            return token;
        }

        private bool IsConstName(string token)
        {
            return _consts.Contains(token);
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
            return _terminalSymbols.Contains(token);
        }

        private bool EvaluateAllowedValues(string token)
        {
            return (AllowedValuesForHighLevelCommands.Count() != 0 ? AllowedValuesForHighLevelCommands.Contains(token) : true) || (AllowLiteralsForHighLevelCommands ? int.TryParse(token, out var value) : false);
        }

        private TokenUnit GetError(string token, TokenUnit tokenUnit)
        {
            tokenUnit.IsValid = false;
            string error = $"\"{token.Split(' ')[0]}\" is not a recognised command, keyword or const name.";

            if (tokenUnit.ErrorMessaes != null)
            {
                tokenUnit.ErrorMessaes.Add(error);
            }
            else
            {
                tokenUnit.ErrorMessaes = new List<string>() { error };
            }
            tokenUnit.StartCharacterOfErrorPosition = tokenUnit.Token.IndexOf(token + 1);

            return tokenUnit;
        }

        private TokenUnit SetError(string error, TokenUnit tokenUnit)
        {
            tokenUnit.IsValid = false;
            if (tokenUnit.ErrorMessaes != null)
            {
                tokenUnit.ErrorMessaes.Add(error);
            }
            else
            {
                tokenUnit.ErrorMessaes = new List<string>() { error };
            }
            tokenUnit.StartCharacterOfErrorPosition = 0;

            return tokenUnit;
        }
    }
}
