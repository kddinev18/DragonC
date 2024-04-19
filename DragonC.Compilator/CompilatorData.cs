using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Compilator
{
    public partial class Compilator
    {
        private List<string> _tokenSeparators = new List<string>() { ";", ":" };
        private List<Command> _commands = new List<Command>()
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
        };
        private List<UnformatedRule> _formalRules = new List<UnformatedRule>()
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
    }
}
