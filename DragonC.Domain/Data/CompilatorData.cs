using DragonC.Domain.Compilator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.Data
{
    public static class CompilatorData
    {
        public static string NonTerminalIndicator { get; set; } = "%";
        public static string DynamicNamesIndicator { get; set; } = "|";
        public static string DynamicValuesIndicator { get; set; } = "@";
        public static string DynamicCommandIndicator { get; set; } = "#";
        public static string DynamicLiteralIndicator { get; set; } = "&";
        public static string CommandJoinSeparator { get; set; } = "^";
        public static string CommandArgumentIndicator { get; set; } = "!";
        public static string DynamicNamesFormalGrammarStartRule { get; set; } = "allowedDynamicNamesStart";
        public static List<string> TokenSeparators { get; set; } = new List<string>() { ";", ":" };
        public static List<LowLevelCommand> LowLevelCommands { get; set; } = new List<LowLevelCommand>();
    }
}
