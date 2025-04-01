using DragonC.Domain.Compilator;
using DragonC.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.HLCC.Services
{
    public interface ICommandPluginProjectService
    {
        HighLevelCommandFile GenerateProject(string[] referenceDllPaths);
        List<HighLevelCommand> LoadHighLevelCommands(CompilatorData data);
    }
}
