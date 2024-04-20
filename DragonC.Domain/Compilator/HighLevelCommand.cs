using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.Compilator
{
    public class HighLevelCommand
    {
        public string CommandDefinition { get; set; }
        public string CommandIndentificatror { get; set; }
        public Func<string, bool> IsCommandValid { get; set; }
        public Func<string, List<string>> CompileCommand { get; set; }

    }
}
