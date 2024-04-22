using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer;

namespace DragonC.Compilator
{
    public partial class Compilator
    {
        public static string NonTerminalIndicator { get; set; } = "%";
        public static string DynamicNamesIndicator { get; set; } = "|";
        public static string DynamicValuesIndicator { get; set; } = "@";
        public static string DynamicCommandIndicator { get; set; } = "#";
        public static string DynamicLiteralIndicator { get; set; } = "&";
        public static string CommandJoinSeparator { get; set; } = "^";
        public static string CommandArgumentIndicator { get; set; } = "!";
        public static List<string> TokenSeparators { get; set; } = new List<string>() { ";", ":" };
        public static List<HighLevelCommand> HighLevelCommands { get; set; } = LoadHighLevelCommands();
        public static List<LowLevelCommand> LowLevelCommands { get; set; } = new List<LowLevelCommand>()
        {
            new LowLevelCommand()
            {
                CommandName = "IMM_TO_REGT",
                MachineCode = "01000101"
            },
            new LowLevelCommand()
            {
                CommandName = "REGT_TO_REG1",
                MachineCode = "01110000"
            },
            new LowLevelCommand()
            {
                CommandName = "REG1_TO_REGT",
                MachineCode = "01000110"
            },
            new LowLevelCommand()
            {
                CommandName = "ADD",
                MachineCode = "10000100"
            },
            new LowLevelCommand()
            {
                CommandName = "REG3_TO_OUT",
                MachineCode = "01011110"
            },
            new LowLevelCommand()
            {
                CommandName = "REG3_TO_REGT",
                MachineCode = "01011101"
            },
            new LowLevelCommand()
            {
                CommandName = "REG2_TO_REGT",
                MachineCode = "01010101"
            },
            new LowLevelCommand()
            {
                CommandName = "REGT_TO_REG2",
                MachineCode = "01110001"
            },
            new LowLevelCommand()
            {
                CommandName = "GO_TO",
                MachineCode = "11000100",
                IsConditionalCommand = true
            },
        };
        public static LowLevelCommand GetCommand(string commandName)
        {
            return LowLevelCommands.First(x => x.CommandName == commandName);
        }
        public static List<UnformatedRule> BaseFormalRules { get; set; } = new List<UnformatedRule>()
        {
            new UnformatedRule
            {
                Rule = "immValue->&_&",
                IsStart = true
            },
            new UnformatedRule
            {
                Rule = "immValueConst->|_|",
                IsStart = true
            },
            new UnformatedRule
            {
                Rule = "commandExec->#_#",
                IsStart = true
            },



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
