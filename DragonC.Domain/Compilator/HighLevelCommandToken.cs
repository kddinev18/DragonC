using DragonC.Domain.Lexer.Tokeniser;

namespace DragonC.Domain.Compilator
{
    public class HighLevelCommandToken
    {
        public HighLevelCommand HighLevelCommand { get; set; }
        public TokenUnit Token { get; set; }
    }
}
