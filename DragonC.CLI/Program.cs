using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer;
using DragonC.Lexer.FormalGrammar;
using DragonC.Lexer.Tokeniser;

namespace DragonC.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //List<UnformatedRule> rules = new List<UnformatedRule>()
            //{
            //    new UnformatedRule("A->var", true),
            //    new UnformatedRule("A->var%B%", false),
            //    new UnformatedRule("B->==%A%", false),
            //    new UnformatedRule("B->!=%A%", false),
            //    new UnformatedRule("B-><%A%", false),
            //    new UnformatedRule("B-><=%A%", false),
            //    new UnformatedRule("B->>=%A%", false),
            //    new UnformatedRule("B->>%A%", false),
            //    new UnformatedRule("B->||%A%", false),
            //    new UnformatedRule("B->&&%A%", false)
            //};

            //FormalGrammar formalGrammar = new FormalGrammar();
            //formalGrammar.SetRules(rules);
            //Console.WriteLine(formalGrammar.CheckToken("varvar"));

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
                    Rule = "2ndSpace-> |dynamicConstValue|%endConstDecl%", 
                },
                new UnformatedRule
                { 
                    Rule = "endConstDecl->;",
                },
                


                new UnformatedRule
                { 
                    Rule = "labelDecl->label%space%", 
                    IsStart = true 
                },
                new UnformatedRule
                {
                    Rule = "space-> %labelName%",
                },
                new UnformatedRule
                { 
                    Rule = "labelName->|dynamicLabelName|%endLabelDecl%", 
                },
                new UnformatedRule
                { 
                    Rule = "endLabelDecl->:", 
                },



                new UnformatedRule
                { 
                    Rule = "commandExec->|dynamicCommandName|%endCommandExec%", 
                    IsStart = true 
                },
                new UnformatedRule
                { 
                    Rule = "endCommandExec->;", 
                },



                new UnformatedRule
                { 
                    Rule = "condCommandExec->|dynamicCondCommandName|%3rdSpace%", 
                    IsStart = true 
                },
                new UnformatedRule
                { 
                    Rule = "3rdSpace-> %condCommandParam%", 
                },
                new UnformatedRule
                { 
                    Rule = "condCommandParam->|dynamicCondCommandParam|%endCondCommandExec%", 
                },
                new UnformatedRule
                { 
                    Rule = "endCondCommandExec->;", 
                },
            };

            FormalGrammar formalGrammar = new FormalGrammar(new List<Command>()
            {
                new Command()
                {
                    CommandName = "comm1",
                }
            });
            Tokeniser tokeniser = new Tokeniser(new List<string>() { ";", ":" });
            List<string> tokens = tokeniser.GetTokens("cosnt test 3;\r\n\r\nlabel main:\r\n\tcomm1;\r\n\tcomm2;\r\njmp main;");
            formalGrammar.SetRules(rules);
            Console.WriteLine(formalGrammar.CheckTokens());
        }
    }
}