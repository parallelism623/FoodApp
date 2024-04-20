using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.DataTransferObjects.Response.V1;

namespace FoodShop.Application.Products.ProductQuery
{
    public static class Query
    {
        public record GetProductsQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder,
                                       IDictionary<string, SortOrder> SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<ProductResponseList>>
        {
            public string cacheKey => $"api:Product:{SearchTerm}:{SortColumn}:{SortOrder}:{SortColumnAndOrder}:{PageIndex}:{PageSize}";
        }

        public record GetProductByIdQuery(Guid Id) : IQuery<ProductResponse>
        {
            public string cacheKey => $"api:Prpduct:{Id}";
        }
    }
}
