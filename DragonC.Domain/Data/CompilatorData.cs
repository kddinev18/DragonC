using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer.FormalGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.Data
{
    public class CompilatorData
    {
        public string NonTerminalIndicator { get; set; } = "%";
        public string DynamicNamesIndicator { get; set; } = "|";
        public string DynamicValuesIndicator { get; set; } = "@";
        public string DynamicCommandIndicator { get; set; } = "#";
        public string DynamicLiteralIndicator { get; set; } = "&";
        public string CommandJoinSeparator { get; set; } = "^";
        public string CommandArgumentIndicator { get; set; } = "!";
        public static string DynamicNamesFormalGrammarStartRule { get; set; } = "allowedDynamicNamesStart";
        public List<string> TokenSeparators { get; set; } = new List<string>();
        public List<LowLevelCommand> LowLevelCommands { get; set; } = new List<LowLevelCommand>();
        public List<UnformatedRule> BaseFormalRules { get; set; } = new List<UnformatedRule>();
        public LowLevelCommand GetCommand(string commandName)
        {
            return LowLevelCommands.First(x => x.CommandName == commandName);
        }
    }
}
