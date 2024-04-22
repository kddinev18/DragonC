using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer;

namespace DragonC.Compilator.HighLevelCommandsCompiler.Base
{
    public abstract class BaseHighLevelCommand
    {
        public abstract HighLevelCommand CommandDefintion { get; set; }
        public abstract TokenUnit ValidateCommand(TokenUnit command);
        public abstract List<string> CompileCommand(TokenUnit command, List<TokenUnit> tokens);
        public abstract string GetClearCommand(string command);
    }
}
