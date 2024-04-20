using DragonC.Domain.Lexer;
using DragonC.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Compilator.HighLevelComandsCompiler
{
    public static class AddOperatorCompilator
    {
        private static List<UnformatedRule> formalRules = new List<UnformatedRule>()
        {
            new UnformatedRule()
            {
                Rule = "addCommandStart=>@_@%space%",
                IsStart = true,
            },
            new UnformatedRule()
            {
                Rule = "space=> %secondParam%"
            },
            new UnformatedRule()
            {
                Rule = "secondParam=>@_@"
            }
        };

        public static List<TokenUnit> IsCommandValid(TokenUnit command)
        {
            FormalGrammar formalGrammar = new FormalGrammar();
            formalGrammar.SetRules(formalRules);

            return formalGrammar.EvaluateTokens(new List<TokenUnit>() { command });
        }

        public static List<TokenUnit> ComplileCommand (TokenUnit command)
        {
            return null;
        }
    }
}
