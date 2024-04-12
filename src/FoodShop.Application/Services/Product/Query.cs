using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.DataTransferObjects.Response.V1;

namespace FoodShop.Application.Services.Product
{
    public static class Query
    {
        public record GetProductsQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder,
                                       IDictionary<string, SortOrder> SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<ProductResponseList>>;
        public record GetProductByIdQuery(Guid Id) : IQuery<ProductResponse>;

    }
}
