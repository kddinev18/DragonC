using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.Lexer.FormalGrammar
{
    public class UnformatedRule
    {
        public string Rule { get; set; }
        public bool IsStart { get; set; } = false;
    }
}
