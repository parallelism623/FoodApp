using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Request.V1
{
    public class PagingListRequest
    {
        public string? SearchTerm { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public string? SortOrderAndColumn { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}

/*
 (string? searchTerm, string? sortColumn, string? sortOrder,
                                               string? sortOrderandColumn, int pageIndex = 1, int pageSize = 10)*/