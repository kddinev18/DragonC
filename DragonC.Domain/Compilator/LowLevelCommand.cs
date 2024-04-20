using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.Compilator
{
    public class LowLevelCommand
    {
        public string CommandName { get; set; }
        public bool IsConditionalCommand { get; set; } = false;
        public string MachineCode { get; set; }
        public string MachineCode16 { get; set; }
    }
}
