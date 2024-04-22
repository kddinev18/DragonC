using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer;

namespace DragonC.Compilator.HighLevelCommandsCompiler
{
    public abstract class BaseHighLevelCommand
    {
        public abstract HighLevelCommand CommandDefintion { get; set; }

        public abstract TokenUnit ValidateCommand(TokenUnit command);

        public abstract List<string> CompileCommand(TokenUnit command);
    }
}
