using DragonC.Domain.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Lexer.FormalGrammar
{
    public class FormalGrammar
    {
        private List<string> _terminalSybols = new List<string>();
        private List<string> _nonTerminalSymbols = new List<string>();
        private List<FormalGrammarRule> _formalGrammarRules = new List<FormalGrammarRule>();
        private Func<UnformatedRule, RuleComponents> _converter;
        private Func<string, string> _formatter;
        private string _startSymbol;

        public string LiteralIndicator { get; set; } = "$";
        public string CommandIndicator { get; set; } = "|";
        public string NonTerminalIndicator { get; set; } = "%";

        public FormalGrammar(Func<UnformatedRule, RuleComponents> converter, Func<string, string> formatter)
        {
            _converter = converter;
            _formatter = formatter;
        }

        public void SetRules(List<UnformatedRule> unformatedRules)
        {
            foreach (UnformatedRule unformatedRule in unformatedRules)
            {
                if (unformatedRule.IsFinal)
                {

                }
            }

            _startSymbol = rules.First().StartNonTerminalSymbol;

            _nonTerminalSymbols = rules
                .SelectMany(x => x.PossibleOutcomes)
                .Select(x => x.NonTerminalPart)
                .Distinct()
                .ToList();
            _terminalSybols = rules
                .SelectMany(x => x.PossibleOutcomes)
                .Select(x => x.TerminalPart)
                .Distinct()
                .ToList();
        }

        private void AddRule(UnformatedRule unformatedRule)
        {
            RuleComponents ruleComponents = _converter.Invoke(unformatedRule);

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
    }
}
