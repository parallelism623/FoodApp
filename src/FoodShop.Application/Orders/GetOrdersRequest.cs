using AutoMapper;
using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.Extensions;
using FoodShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Orders
{
    public record GetOrdersQuery(PagingListRequest Model) : IQuery<PagedResult<OrderListResponse>>;

    public class GetOrderHandler : IQueryHandler<GetOrdersQuery, PagedResult<OrderListResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IQueryRepository _queryRepository;
        public GetOrderHandler(IMapper mapper, IQueryRepository queryRepository)
        {
            _mapper = mapper;
            _queryRepository = queryRepository;
        }
        public async Task<Result<PagedResult<OrderListResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
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
            sqlQueryString.Remove(sqlQueryString.Length - 2);
            var resultQuery = (await _queryRepository.QueryAsync<Order>(sqlQueryString));
            var resultMap = _mapper.Map<IQueryable<OrderListResponse>>(resultQuery);
            var finalResult = await PagedResult<OrderListResponse>.CreateAsync(resultMap, pageIndex, pageSize);
            return finalResult;
        } 
    }
}
