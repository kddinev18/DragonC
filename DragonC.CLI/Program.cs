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
                new UnformatedRule("constDecl->cosnt%space%", false),
                new UnformatedRule("space-> |dynamicConstName|%2ndSpace%", false),
                new UnformatedRule("2ndSpace-> |dynamicConstValue|%endConstDecl%", false),
                new UnformatedRule("endConstDecl->;", true),


                new UnformatedRule("labelDecl->label%labelName%", false),
                new UnformatedRule("labelName->|dynamicLabelName|%endLabelDecl%", false),
                new UnformatedRule("endLabelDecl->:", true),



                new UnformatedRule("commandExec->|dynamicCommandName|%endCommandExec%", false),
                new UnformatedRule("endCommandExec->;", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
                new UnformatedRule("", false),
            };

            FormalGrammar formalGrammar = new FormalGrammar();
            formalGrammar.SetRules(rules);
            Console.WriteLine(formalGrammar.CheckToken("cosnt |dynamicConstName| |dynamicConstValue|;"));
        }
    }
}