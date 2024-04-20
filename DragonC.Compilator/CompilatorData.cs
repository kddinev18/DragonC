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
        public string NonTerminalIndicator { get; set; } = "%";
        public string DynamicNamesIndicator { get; set; } = "|";
        public string DynamicValuesIndicator { get; set; } = "@";
        public string DynamicCommandIndicator { get; set; } = "#";
        public string CommandJoinSeparator { get; set; } = "^";

        private List<string> _tokenSeparators = new List<string>() { ";", ":" };
        private List<Command> _commands = new List<Command>()
        {
            new Command()
            {
                CommandName = "IMM_TO_REGT",
                MachineCode = "01000101"
            },
            new Command()
            {
                CommandName = "REGT_TO_REG1",
                MachineCode = "01110000"
            },
            new Command()
            {
                CommandName = "ADD",
                MachineCode = "10000100"
            },
            new Command()
            {
                CommandName = "REG3_TO_OUT",
                MachineCode = "01011110"
            },
            new Command()
            {
                CommandName = "REG3_TO_REGT",
                MachineCode = "01011101"
            },
            new Command()
            {
                CommandName = "REGT_TO_REG2",
                MachineCode = "01110001"
            },
            new Command()
            {
                CommandName = "GO_TO",
                MachineCode = "11000100",
                IsConditionalCommand = true
            },
        };
        private List<UnformatedRule> _formalRules = new List<UnformatedRule>()
        {
            new UnformatedRule()
            {
                Rule = "constDecl->const%space%",
                IsStart = true
            },
            new UnformatedRule
            {
                Rule = "space-> %cnastName%",
            },
            new UnformatedRule
            {
                Rule = "cnastName->|_|%2ndSpace%"
            },
            new UnformatedRule
            {
                Rule = "2ndSpace-> %constValue%",
            },
            new UnformatedRule
            {
                Rule = "constValue->@_@",
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
                Rule = "labelName->|_|",
            },



            new UnformatedRule
            {
                Rule = "commandExec->#_#",
                IsStart = true
            },



            new UnformatedRule
            {
                Rule = "immValue->|_|",
                IsStart = true
            },



            new UnformatedRule
            {
                Rule = "condCommandExec->#_#%spaceCmd%",
                IsStart = true
            },
            new UnformatedRule
            {
                Rule = "spaceCmd-> %condCommandParam%",
            },
            new UnformatedRule
            {
                Rule = "condCommandParam->@_@",
            }
        };
    }
}
