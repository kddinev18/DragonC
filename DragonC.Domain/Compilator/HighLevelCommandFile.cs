using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.Compilator
{
    public class HighLevelCommandFile
    {
        public string FileName { get; set; }
        public string ProjectPath { get; set; }
        public string FileContent { get; set; }
        public string FullFilePath { get; set; }
    }
}
