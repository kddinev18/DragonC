﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.API
{
    public class ProjectDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public FileInfo ProcessorFile { get; set; }
    }
}
