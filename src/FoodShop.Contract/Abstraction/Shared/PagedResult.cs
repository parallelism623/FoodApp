using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Shared
{
    public class PagedResult<T>
    {
        public const int UpperPageSize = 100;
        public const int DefaultPageSize = 10;
        public const int DefaultPageIndex = 1;
        public PagedResult(List<T> items, int pageIndex, int pageSize, int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public int PageIndex { get; }
        public int PageSize { get; }
        public List<T> Items { get; }
        public int TotalCount { get; }
        public bool HasNextPage => PageIndex * PageSize < TotalCount;
        public int MaxPageIndex => (TotalCount + PageSize - 1) / PageSize;
        public bool HasPreviousPage => PageIndex > 1;

        public static async Task<PagedResult<T>> CreateAsync(IQueryable<T> query, int pageIndex, int pageSize)
        {
            pageIndex = pageIndex <= 0 ? DefaultPageIndex : pageIndex;
            pageSize = pageSize <= 0 ?
                       DefaultPageIndex :
                       pageSize > UpperPageSize ?
                       UpperPageSize : pageSize;
            var totalCount = query.Count();
            var items = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new(items, pageIndex, pageSize, totalCount);
        }
    }
}
