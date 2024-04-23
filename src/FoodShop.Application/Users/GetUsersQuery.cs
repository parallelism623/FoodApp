using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;


namespace FoodShop.Application.Users
{
    public record GetUsersQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder,
                                    IDictionary<string, SortOrder> SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<UserResponseList>>
    {
        public string cacheKey => $"api:User:{SearchTerm}:{SortColumn}:{SortOrder}:{SortColumnAndOrder}:{PageIndex}:{PageSize}";
    }

}
