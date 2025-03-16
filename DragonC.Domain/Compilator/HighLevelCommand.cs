using DragonC.Domain.Lexer.FormalGrammar;
using DragonC.Domain.Lexer.Tokeniser;

namespace DragonC.Domain.Compilator
{
    public class HighLevelCommand
    {
        public string CommandDefinition { get; set; }
        public string CommandIndentificatror { get; set; }
        public string CommandSeparator { get; set; }
        public List<string> Arguments { get; set; }
        public List<string> AllowedValuesForArguments { get; set; } = new List<string>();
        public bool AllowLiteralsForArguments { get; set; }
        public List<UnformatedRule> FormalRules { get; set; }

        public Func<TokenUnit, TokenUnit> ValidateCommand { get; set; }
        public Func<TokenUnit, List<TokenUnit>, List<string>> CompileCommand { get; set; }
        public Func<string, string> GetClearCommand { get; set; }
        public Action<List<TokenUnit>> SetConsts { get; set; } = null;
    }
}
