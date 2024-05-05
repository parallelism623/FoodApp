using AutoMapper;
using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Application.Identity.Users;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.Extensions;
using FoodShop.Domain.Entities;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Orders
{
    public record GetOrdersByUserIdQuery(Guid UserId, PagingListRequest Model) : IQuery<PagedResult<OrderListResponse>>;

    public class GetOrdersByUserIdHandler : IQueryHandler<GetOrdersByUserIdQuery, PagedResult<OrderListResponse>>
    {
        private readonly IUserServices _userServices;
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        public GetOrdersByUserIdHandler(IQueryRepository queryRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<OrderListResponse>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {

            await _userServices.IsUserExists(request.UserId);
            var model = request.Model;
            var pageIndex = model.PageIndex <= 0 ? PagedResult<OrderListResponse>.DefaultPageIndex
                : model.PageIndex;
            var pageSize = model.PageSize <= 0 ? PagedResult<OrderListResponse>.DefaultPageSize
                : (model.PageSize > PagedResult<OrderListResponse>.UpperPageSize) ? PagedResult<OrderListResponse>.UpperPageSize
                : model.PageSize;
            //Search -> Sort -> Paging;
            var sqlQueryString = !string.IsNullOrEmpty(model.SearchTerm)
                ? $"SELECT * FROM {nameof(Order)} WHERE {nameof(Order.Title)} LIKE '%{model.SearchTerm}%'  "
                : $"SELECT * FROM {nameof(Order)}  ";
            sqlQueryString += $"AND {nameof(Order.UserId)} LIKE '{request.UserId.ToString()}'";
            if (!string.IsNullOrEmpty(model.SortColumn) || !string.IsNullOrEmpty(model.SortOrderAndColumn))
            {
                sqlQueryString.Remove(sqlQueryString.Length - 1);
                sqlQueryString += "ORDER BY";
            }
            var sortOrder = SortOrderExtensions.ConvertStringToSortOrder(model.SortOrder);
            sqlQueryString += sortOrder == SortOrder.Ascending
                ? $" {OrderExtensions.GetSortOrderProperty(model.SortOrder)} ASC, "
                : $" {OrderExtensions.GetSortOrderProperty(model.SortOrder)} DESC, ";
            var sortOrderAndColumns = SortOrderExtensions.ConvertStringToDictSortOrder(model.SortOrderAndColumn);
            foreach (var items in sortOrderAndColumns)
            {
                sqlQueryString += sortOrder == SortOrder.Ascending
                    ? $"{OrderExtensions.GetSortOrderProperty(items.Key)} ASC, "
                    : $"{OrderExtensions.GetSortOrderProperty(items.Key)} DESC, ";
            }
            sqlQueryString += $"OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageIndex} ROWS ONLY, ";
            sqlQueryString.Remove(sqlQueryString.Length - 2);
            var resultQuery = (await _queryRepository.QueryAsync<Order>(sqlQueryString));
            var resultMap = _mapper.Map<IQueryable<OrderListResponse>>(resultQuery);
            var finalResult = await PagedResult<OrderListResponse>.CreateAsync(resultMap, pageIndex, pageSize);
            return finalResult;
        }
    }
}
