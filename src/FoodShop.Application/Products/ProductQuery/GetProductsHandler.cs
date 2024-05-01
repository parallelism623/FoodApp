using AutoMapper;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using System.Linq.Expressions;

using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Application.Common.Caching;

namespace FoodShop.Application.Products.ProductQuery
{
    public class GetProductsHandler : IQueryHandler<GetProductsQuery, PagedResult<ProductResponseList>>
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        private readonly ICacheServices _cacheServices;
        
        public GetProductsHandler(
            IQueryRepository queryRepository,
            IMapper mapper,
            ICacheServices cacheServices)
        {
            _cacheServices = cacheServices;
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<ProductResponseList>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var paramString =
                $"{request.SearchTerm}:{request.SortOrder}:{request.SortOrder}:{request.SortColumnAndOrder}";
            var cacheKey = _cacheServices.GetCacheKey(string.Empty, nameof(GetProductsQuery), paramString);
            var pageIndex = request.PageIndex <= 0 ? PagedResult<ProductResponseList>.DefaultPageIndex : request.PageIndex;
            var pageSize = request.PageSize <= 0 ? PagedResult<ProductResponseList>.DefaultPageSize :
                            request.PageSize > PagedResult<ProductResponseList>.UpperPageSize ?
                            PagedResult<ProductResponseList>.UpperPageSize : request.PageSize;
            var productQuery = string.IsNullOrEmpty(request.SearchTerm) ?
                                $@"SELECT * FROM {nameof(Product)} ORDER BY " :
                                $@"SELECT * FROM {nameof(Product)} 
                                    WHERE {nameof(FoodShop.Domain.Entities.Product.Name)} LIKE '%{request.SearchTerm}%'
                                    OR WHERE {nameof(FoodShop.Domain.Entities.Product.Description)} LIKE '%{request.SearchTerm}%'
                                    ORDER BY ";
            foreach (var items in request.SortColumnAndOrder)
            {
                productQuery += items.Value == SortOrder.Ascending
                    ? $"{items.Key} ASC, " : $"{items.Key} DESC, ";
            }
            productQuery = productQuery.Remove(productQuery.Length - 2);

            productQuery += $" OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            var products = await _queryRepository.QueryAsync<FoodShop.Domain.Entities.Product>(productQuery);
            var totalCount = products.Count();
            var productsResult = _mapper.Map<List<ProductResponseList>>(products);
            var result = new PagedResult<ProductResponseList>(productsResult, pageIndex, pageSize, totalCount);
            return result;

        }
        public static Expression<Func<FoodShop.Domain.Entities.Product, object>> GetSortProperty(string? sortColumn)
            => sortColumn?.ToLower().Trim() switch
            {
                "name" => product => product.Name,
                "price" => product => product.Price,
                "description" => product => product.Description,
                _ => product => product.Id
            };
    }
}
