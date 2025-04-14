using DragonC.Domain.Compilator;
using DragonC.Domain.Lexer.FormalGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.API
{
    public class CodeConfigurationDTO
    {
		public int ProjectId { get; set; }
		public List<string> TokenSeparators { get; set; } = new List<string>();
        public List<LowLevelCommand> LowLevelCommands { get; set; } = new List<LowLevelCommand>();
        public List<UnformatedRule> BaseFormalRules { get; set; } = new List<UnformatedRule>();
        public List<string> HighLevelCommandsCode { get; set; } = new List<string>();
    }
}
