using DragonC.Domain.Lexer;
using DragonC.Lexer.FormalGrammar;

namespace DragonC.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<UnformatedRule> rules = new List<UnformatedRule>()
            {
                new UnformatedRule("A->var", true),
                new UnformatedRule("A->var%B%", false),
                new UnformatedRule("B->==%A%", false),
                new UnformatedRule("B->!=%A%", false),
                new UnformatedRule("B-><%A%", false),
                new UnformatedRule("B-><=%A%", false),
                new UnformatedRule("B->>=%A%", false),
                new UnformatedRule("B->>%A%", false),
                new UnformatedRule("B->||%A%", false),
                new UnformatedRule("B->&&%A%", false)
            };

            FormalGrammar formalGrammar = new FormalGrammar();
            formalGrammar.SetRules(rules);


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