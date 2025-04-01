using DragonC.Domain.Compilator;
using DragonC.Domain.Data;
using DragonC.GUI.Components.HighLevelCommandsComponent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Services.Contracts
{
    public interface ICommandPluginProjectService
    {
        HighLevelCommandFile GenerateProject(string[] referenceDllPaths);
        List<HighLevelCommand> LoadHighLevelCommands(CompilatorData data, string pluginFolder, string[] referencePaths);
    }
}
