using DragonC.Domain.Lexer;

namespace DragonC.Compilator.HighLevelCommandsCompiler.Base
{
    public interface IAllowConsts
    {
        public void SetConsts(List<TokenUnit> tokens);
    }
}
