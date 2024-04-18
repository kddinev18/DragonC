using DragonC.Domain.Lexer;

namespace DragonC.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<UnformatedRule> rules = new List<UnformatedRule>()
            {
                new UnformatedRule("A->var", true),
                new UnformatedRule("A->varB", false),
                new UnformatedRule("B->==A", false),
                new UnformatedRule("B->!=A", false),
                new UnformatedRule("B-><A", false),
                new UnformatedRule("B-><=A", false),
                new UnformatedRule("B->>=A", false),
                new UnformatedRule("B->>A", false),
                new UnformatedRule("B->||A", false),
                new UnformatedRule("B->&&A", false),
            }


            //std::cout << formalGrammar.checkWord("var") << std::endl;
            //std::cout << formalGrammar.checkWord("varvar") << std::endl;
            //std::cout << formalGrammar.checkWord("var||var") << std::endl;
            //std::cout << formalGrammar.checkWord("var||var!=var") << std::endl;
            //std::cout << formalGrammar.checkWord("var==<var") << std::endl;
            //std::cout << formalGrammar.checkWord("var<<var") << std::endl;
            //std::cout << formalGrammar.checkWord("var<=") << std::endl;
        }
    }
}