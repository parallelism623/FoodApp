using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Request.V1
{
    public class ListRequest
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string SortOrderAndColumn { get; set; }
        public string SearchTerm { get; set; }

    }
}
