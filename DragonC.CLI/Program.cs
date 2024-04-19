using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer;
using DragonC.Lexer;

namespace DragonC.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<UnformatedRule> rules = new List<UnformatedRule>()
            {
                new UnformatedRule()
                {
                    Rule = "constDecl->cosnt%space%",
                    IsStart = true
                },
                new UnformatedRule
                {
                    Rule = "space-> |dynamicConstName|%2ndSpace%",
                },
                new UnformatedRule
                { 
                    Rule = "2ndSpace->|dynamicConstValue|", 
                },
                


                new UnformatedRule
                { 
                    Rule = "labelDecl->label%spaceLbl%", 
                    IsStart = true 
                },
                new UnformatedRule
                {
                    Rule = "spaceLbl-> %labelName%",
                },
                new UnformatedRule
                { 
                    Rule = "labelName->|dynamicLabelName|", 
                },



                new UnformatedRule
                { 
                    Rule = "commandExec->|dynamicCommandName|", 
                    IsStart = true 
                },



                new UnformatedRule
                { 
                    Rule = "condCommandExec->|dynamicCondCommandName|%spaceCmd%", 
                    IsStart = true 
                },
                new UnformatedRule
                { 
                    Rule = "spaceCmd-> %condCommandParam%", 
                },
                new UnformatedRule
                { 
                    Rule = "condCommandParam->|dynamicCondCommandParam|", 
                }
            };

            FormalGrammar formalGrammar = new FormalGrammar(new List<Command>()
            {
                new Command()
                {
                    CommandName = "comm1",
                },
                new Command()
                {
                    CommandName = "comm2",
                },
                new Command()
                {
                    CommandName = "jmp",
                    IsConditionalCommand = true
                },
            });
            Tokeniser tokeniser = new Tokeniser(new List<string>() { ";", ":" });
            List<TokenUnit> tokens = tokeniser.GetTokens("const test 3;\r\n\r\nlabel main:\r\n\tcomm1;\r\n\tcomm2;\r\njmp main;");
            formalGrammar.SetRules(rules);
            tokens = formalGrammar.EvaluateTokens(tokens);
        }
    }
}