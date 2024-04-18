namespace DragonC.Domain.Lexer
{
    public class FormalGrammarRule
    {
        public string StartNonTerminalSymbol { get; set; }
        public List<Rule> PossibleOutcomes { get; set; }
    }
}