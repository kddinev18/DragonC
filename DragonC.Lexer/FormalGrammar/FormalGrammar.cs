using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer;
using System.Text;

namespace DragonC.Lexer.FormalGrammar
{
    public class FormalGrammar
    {
        private List<string> _terminalSybols = new List<string>();
        private List<string> _nonTerminalSymbols = new List<string>();
        private List<FormalGrammarRule> _formalGrammarRules = new List<FormalGrammarRule>();
        public string NonTerminalIndicator { get; set; } = "%";

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

            return new RuleComponents()
            {
                StartSymvol = ruleComponents[0],
                TerminalPart = ruleComponents[1].Split(NonTerminalIndicator)[0],
                NonTerminalPart = unformatedRule.IsFinal ? "" : ruleComponents[1].Split(NonTerminalIndicator)[1],
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

        public bool CheckTokens(List<string> tokens)
        {
            bool flag = true;
            foreach (string token in tokens)
            {
                flag = CheckToken(token);
                if (!flag)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckToken(string token)
        {
            foreach (var startFormalRule in _formalGrammarRules.Where(x => x.IsStart))
            {
                int possibelOutcomeIndex = 0;
                FormalGrammarRule currentFormlRule = startFormalRule;
                Rule rule = currentFormlRule.PossibleOutcomes[0];
                while (true)
                {
                    if (currentFormlRule.PossibleOutcomes.Count() == possibelOutcomeIndex - 1)
                    {
                        return false;
                    }
                    if (rule.Next == null)
                    {
                        if (rule.TerminalPart == token)
                        {
                            return true;
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
                                return false;
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
                                    return true;
                                }
                                return false;
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
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
