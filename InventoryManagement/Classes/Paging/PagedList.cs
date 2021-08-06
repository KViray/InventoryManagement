using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Classes.Paging
{
    public class PagedList<T>: List<T>
    {
        public dynamic Result { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int Limit { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(IEnumerable<T> data, int count, int pageNumber, int limit)
        {
            Result = data;
            TotalCount = count;
            Limit = limit;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)limit);
            AddRange(data);
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int limit)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * limit)
                .Take(limit).ToList();
            return new PagedList<T>(items, count, pageNumber, limit);
        }
    }
}
