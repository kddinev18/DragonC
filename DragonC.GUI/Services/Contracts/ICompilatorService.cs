using DragonC.Domain.Compilator;
using DragonC.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Services.Contracts
{
    public interface ICompilatorService
    {
        public CompilatorData CompilatorData { get; set; }
        public CompiledCode Compile(string code);
    }
}
