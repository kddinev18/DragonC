using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.API
{
	public class FileDTO
	{
		public string? Name { get; set; }
		public byte[]? FileData { get; set; }
	}
}
