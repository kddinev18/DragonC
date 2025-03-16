namespace DragonC.Domain.Lexer.FormalGrammar
{
    public class FormalGrammarRule
    {
        public string StartNonTerminalSymbol { get; set; }
        public List<Rule> PossibleOutcomes { get; set; }
        public bool IsStart { get; set; }
    }
}