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
        private Func<UnformatedRule, FormalGrammarRule> _formatter;
        private string _startSymbol;

        public string LiteralIndicator { get; set; } = "$value$";
        public string CommandIndicator { get; set; } = "$cmd";

        public FormalGrammar(Func<UnformatedRule, FormalGrammarRule> formatter)
        {
            _formatter = formatter;
        }

        public void SetRules(List<UnformatedRule> unformatedRules)
        {
            List<FormalGrammarRule> rules = new List<FormalGrammarRule>();
            foreach (var unformatedRule in unformatedRules)
            {
                rules.Add(_formatter.Invoke(unformatedRule));
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
    }
}
