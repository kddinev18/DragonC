using DragonC.Domain.Lexer;
using System.Text;

namespace DragonC.Lexer.FormalGrammar
{
    public class FormalGrammar
    {
        private List<string> _terminalSybols = new List<string>();
        private List<string> _nonTerminalSymbols = new List<string>();
        private List<FormalGrammarRule> _formalGrammarRules = new List<FormalGrammarRule>();
        private string _startSymbol;

        public string LiteralIndicator { get; set; } = "$";
        public string CommandIndicator { get; set; } = "|";
        public string NonTerminalIndicator { get; set; } = "%";

        public void SetRules(List<UnformatedRule> unformatedRules)
        {
            foreach (UnformatedRule unformatedRule in unformatedRules)
            {
                AddRule(unformatedRule);
            }
            LinkRules();
            _startSymbol = _formalGrammarRules.First().StartNonTerminalSymbol;

            _nonTerminalSymbols = _formalGrammarRules
                .SelectMany(x => x.PossibleOutcomes)
                .Select(x => x.NonTerminalPart)
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


        private string FormatRule(string rule)
        {
            StringBuilder newRule = new StringBuilder();
            for (int i = 0; i < rule.Length; i++)
            {
                if (rule[i] != ' ' && rule[i] != '\n' && rule[i] != '\r')
                {
                    newRule.Append(rule[i]);
                }
            }

            return newRule.ToString();
        }

        private RuleComponents GetRuleComponents(UnformatedRule unformatedRule)
        {
            string[] ruleComponents = FormatRule(unformatedRule.Rule).Split("->");
            string terminals = ruleComponents[1];

            return new RuleComponents()
            {
                StartSymvol = ruleComponents[0],
                TerminalPart = ruleComponents[1].Split(NonTerminalIndicator)[0],
                NonTerminalPart = unformatedRule.IsFinal ? "" : ruleComponents[1].Split(NonTerminalIndicator)[1]
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
    }
}
