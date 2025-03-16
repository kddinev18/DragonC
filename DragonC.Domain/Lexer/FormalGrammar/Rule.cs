using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.Lexer.FormalGrammar
{
    public class Rule
    {
        public string TerminalPart { get; set; }
        public string NonTerminalPart { get; set; }
        public FormalGrammarRule Next { get; set; }

        public bool IsFinal => string.IsNullOrEmpty(NonTerminalPart);
    }
}
