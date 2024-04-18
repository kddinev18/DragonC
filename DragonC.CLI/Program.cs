using DragonC.Domain.Lexer;
using DragonC.Lexer.FormalGrammar;

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
                    IsFinal = false,
                    IsStart = true
                },
                new UnformatedRule
                {
                    Rule = "space-> |dynamicConstName|%2ndSpace%",
                    IsFinal = false,
                    IsStart = false
                },
                new UnformatedRule
                { 
                    Rule = "2ndSpace-> |dynamicConstValue|%endConstDecl%", 
                    IsFinal = false, 
                    IsStart = false 
                },
                new UnformatedRule
                { 
                    Rule = "endConstDecl->;",
                    IsFinal = true,
                    IsStart = false
                },
                


                new UnformatedRule
                { 
                    Rule = "labelDecl->label%space%", 
                    IsFinal = false, 
                    IsStart = true 
                },
                new UnformatedRule
                {
                    Rule = "space-> %labelName%",
                    IsFinal = false,
                    IsStart = true
                },
                new UnformatedRule
                { 
                    Rule = "labelName->|dynamicLabelName|%endLabelDecl%", 
                    IsFinal = false, 
                    IsStart = false 
                },
                new UnformatedRule
                { 
                    Rule = "endLabelDecl->:", 
                    IsFinal = true, 
                    IsStart = false 
                },



                new UnformatedRule
                { 
                    Rule = "commandExec->|dynamicCommandName|%endCommandExec%", 
                    IsFinal = false, 
                    IsStart = true 
                },
                new UnformatedRule
                { 
                    Rule = "endCommandExec->;", 
                    IsFinal = true, 
                    IsStart = false 
                },



                new UnformatedRule
                { 
                    Rule = "condCommandExec->|dynamicCondCommandName|%3rdSpace%", 
                    IsFinal = false, 
                    IsStart = true 
                },
                new UnformatedRule
                { 
                    Rule = "3rdSpace-> %condCommandParam%", 
                    IsFinal = false, 
                    IsStart = false 
                },
                new UnformatedRule
                { 
                    Rule = "condCommandParam->|dynamicCondCommandParam|%endCondCommandExec%", 
                    IsFinal = false, 
                    IsStart = false 
                },
                new UnformatedRule
                { 
                    Rule = "endCondCommandExec->;", 
                    IsFinal = true, 
                    IsStart = false 
                },
            };

            FormalGrammar formalGrammar = new FormalGrammar();
            formalGrammar.SetRules(rules);
            Console.WriteLine(formalGrammar.CheckToken("|dynamicCondCommandName| |dynamicCondCommandParam|;"));
        }
    }
}