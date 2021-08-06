using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Classes.Paging
{
    public class Paging
    {
        public int? Page { get; set; }
        public int? Limit { get; set; }
    }

    public class PagedData
    {
        public dynamic Result { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int Limit { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }

}
