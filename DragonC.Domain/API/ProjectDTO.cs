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
        public string? Name { get; set; }
		public string? FileName { get; set; }
		public byte[]? FileData { get; set; }
	}
}
