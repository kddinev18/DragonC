using DragonC.Domain.Compilator;
using DragonC.Domain.Data;
using DragonC.GUI.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Services
{
    public class CompilatorService : ICompilatorService
    {
        public CompilatorData CompilatorData { get; set; }

        public CompiledCode Compile(string code)
        {
            if(CompilatorData == null)
            {
                return new CompiledCode()
                {
                    ErrorMessages = new List<string>() { "Instructions not set. Please set the instructions for the code you want to write before compiling it" }
                };
            }

            return new Compilator.Compilator(CompilatorData).Compile(code);
        }
    }
}
