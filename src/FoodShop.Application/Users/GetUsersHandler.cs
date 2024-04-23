using AutoMapper;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Users
{
    public class GetUsersHandler : IQueryHandler<GetUsersQuery, PagedResult<UserResponseList>>
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        public async Task<Result<PagedResult<UserResponseList>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var pageSize = request.PageSize <= 0 ? PagedResult<UserResponseList>.DefaultPageSize
                           : request.PageSize > PagedResult<UserResponseList>.UpperPageSize ?
                           PagedResult<UserResponseList>.UpperPageSize : request.PageSize;
            var pageIndex = request.PageIndex <= 0 ? PagedResult<UserResponseList>.DefaultPageIndex : request.PageIndex;
            var querySql = string.IsNullOrEmpty(request.SearchTerm) ?
                           $@"SELECT * FROM {nameof(AppUser)} ORDER BY" :
                           $@"SELECT * FROM {nameof(AppUser)} WHERE {nameof(AppUser.FirstName)} LIKE '%{request.SearchTerm}%'
                              OR WHERE {nameof(AppUser.LastName)} LIKE '%{request.SearchTerm}%' ORDER BY";
            foreach(var items in request.SortColumnAndOrder)
            {
                querySql += items.Value == SortOrder.Ascending ? 
                            $"{nameof(items.Key)} ASC" : $"{nameof(items.Key)} DESC, ";
            }
            querySql = querySql.Remove(querySql.Length - 2);
            querySql += $"OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            var result = await _queryRepository.QueryAsync<AppUser>(querySql);
            var mapResult = _mapper.Map<List<UserResponseList>>(result);
            return new PagedResult<UserResponseList>(mapResult, pageIndex, pageSize, result.Count());
        }
    }
}
