using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.Domain.API.Common
{
    public class PagedCollection<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public T Filters { get; set; }
    }
}
