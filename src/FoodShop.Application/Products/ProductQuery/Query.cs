using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;

namespace FoodShop.Application.Products.ProductQuery
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
