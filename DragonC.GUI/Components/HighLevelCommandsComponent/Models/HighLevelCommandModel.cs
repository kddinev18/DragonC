using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Components.HighLevelCommandsComponent.Models
{
    public class HighLevelCommandModel
    {
        public HighLevelCommandModel(HighLevelCommandModel model)
        {
            FileName = model.FileName;
            FileContent = model.FileContent;
            FullFilePath = model.FullFilePath;
            CommandName = model.CommandName;
        }

        public HighLevelCommandModel()
        {

        }
        public string FileName { get; set; }
        public string FileContent { get; set; }
        public string FullFilePath { get; set; }
        public string CommandName { get; set; }
    }
}
