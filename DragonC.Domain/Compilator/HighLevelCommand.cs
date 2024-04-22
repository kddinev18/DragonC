using DragonC.Domain.Lexer;

namespace DragonC.Domain.Compilator
{
    public class HighLevelCommand
    {
        public string CommandDefinition { get; set; }
        public string CommandIndentificatror { get; set; }
        public string CommandSeparator { get; set; }
        public List<string> Arguments { get; set; }
        public List<string> AllowedValuesForArguments { get; set; }
        public bool AllowLiteralsForArguments { get; set; }
        public List<UnformatedRule> FormalRules { get; set; }

        public Func<TokenUnit, TokenUnit> ValidateCommand { get; set; }
        public Func<TokenUnit, List<LowLevelCommand>> CompileCommand { get; set; }
    }
}
