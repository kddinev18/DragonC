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

        public static List<string> registers = new List<string>()
        {
            "REG0",
            "REG1",
            "REG2",
            "REG3",
            "REG4",
            "REG5",
        };

        public static List<TokenUnit> IsCommandValid(TokenUnit command)
        {
            FormalGrammar formalGrammar = new FormalGrammar();
            formalGrammar.SetRules(formalRules);
            formalGrammar.AllowLiteralsForHighLevelCommands = true;
            formalGrammar.AllowedValuesForHighLevelCommands = registers;

            return formalGrammar.EvaluateTokens(new List<TokenUnit>() { command });
        }

        public static List<TokenUnit> ComplileCommand (TokenUnit command)
        {
            return null;
        }
    }
}
