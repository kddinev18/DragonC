using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.Compilator
{
    public class CompiledCode
    {
        public bool IsBuildSuccessfully { get; set; }
        public List<string>? CompiledCommands { get; set; }
        public List<string>? ErrorMessages { get; set; }
    }
}
