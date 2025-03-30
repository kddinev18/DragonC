using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Services.Contracts
{
    public interface ICommandPluginProjectService
    {
        string GenerateProject(string[] referenceDllPaths);
    }
}
