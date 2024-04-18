using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.Lexer
{
    public class RuleComponents
    {
        public string StartSymvol { get; set; }
        public string TerminalPart { get; set; }
        public string NonTerminalPart { get; set; }
        public bool IsStart { get; set; }
    }
}
