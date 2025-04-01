using DragonC.Domain.Compilator;
using DragonC.GUI.Services;
using DragonC.GUI.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonC.HLCC.Services;

namespace DragonC.GUI.Components.HighLevelCommandsComponent.Models
{
    public class HighLevelCommandModel
    {
        public HighLevelCommandModel(bool asd)
        {
            
        }
        public HighLevelCommandModel()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] references =
            {
                Path.Combine(baseDir, "DragonC.GUI.exe"),
                Path.Combine(baseDir, "DragonC.Domain.dll"),
                Path.Combine(baseDir, "DragonC.Lexer.dll"),
                Path.Combine(baseDir, "DragonC.Domain.Compilator.dll")
            };

            ICommandPluginProjectService service = new CommandPluginProjectService();
            HighLevelCommandFile path = service.GenerateProject(references);
            FileName = path.FileName;
            ProjectPath = path.ProjectPath;
            FileContent = path.FileContent;
            FullFilePath = path.FullFilePath;
        }
        public string FileName { get; set; }
        public string ProjectPath { get; set; }
        public string FileContent { get; set; }
        public string FullFilePath { get; set; }
        public string CommandName { get; set; }
    }
}
