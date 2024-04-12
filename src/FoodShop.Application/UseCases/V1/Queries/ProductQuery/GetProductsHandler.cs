using AutoMapper;
using Azure;
using FoodShop.Application.Services.Product;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.DataTransferObjects.Response.V1;
using FoodShop.Domain.Abstraction.Repositories;
using FoodShop.Contract.Extensions;
using FoodShop.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using FoodShop.Domain.Entities;

namespace FoodShop.Application.UseCases.V1.Queries.ProductQuery
{
    public class GetProductsHandler : IQueryHandler<Query.GetProductsQuery, PagedResult<ProductResponseList>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public GetProductsHandler(IProductRepository productRepository,
                                  ApplicationDbContext context,
                                  IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _context = context;

        }

        public async Task<Result<PagedResult<ProductResponseList>>> Handle(Query.GetProductsQuery request, CancellationToken cancellationToken)
        {
            if (request.SortColumnAndOrder.Any())
            {
                var pageIndex = request.PageIndex <= 0 ? PagedResult<ProductResponseList>.DefaultPageIndex : request.PageIndex; 
                var pageSize = request.PageSize <= 0 ? PagedResult<ProductResponseList>.DefaultPageSize :
                               request.PageSize > PagedResult<ProductResponseList>.UpperPageSize ?
                               PagedResult<ProductResponseList>.UpperPageSize : request.PageSize;
                var productQuery = string.IsNullOrEmpty(request.SearchTerm) ?
                                   $@"SELECT * FROM {nameof(FoodShop.Domain.Entities.Product)} ORDER BY " :
                                   $@"SELECT * FROM {nameof(FoodShop.Domain.Entities.Product)} 
                                        WHERE {nameof(FoodShop.Domain.Entities.Product.Name)} LIKE '%{request.SearchTerm}%'
                                        OR WHERE {nameof(FoodShop.Domain.Entities.Product.Description)} LIKE '%{request.SearchTerm}%'
                                        ORDER BY ";
                foreach(var items in request.SortColumnAndOrder)
                {
                    productQuery += items.Value == Contract.Abstraction.Constrant.SortOrder.Ascending
                        ? $"{items.Key} ASC, " : $"{items.Key} DESC, ";
                }
                productQuery = productQuery.Remove(productQuery.Length - 2);
                
                productQuery += $" OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
                var products = await _context.Products.FromSqlRaw(productQuery)
                                                      .ToListAsync(cancellationToken: cancellationToken);
                var totalCount = products.Count();
                var productsResult = _mapper.Map<List<ProductResponseList>>(products);
                var result = new PagedResult<ProductResponseList>(productsResult, pageIndex, pageSize, totalCount);
                return result;
            }
            else
            {
                var productsQuery = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? _productRepository.FindAll()
                : _productRepository.FindAll(x => x.Name.Contains(request.SearchTerm) || x.Description.Contains(request.SearchTerm));

                productsQuery = request.SortOrder == SortOrder.Descending
                ? productsQuery.OrderByDescending(GetSortProperty(request.SortColumn))
                : productsQuery.OrderBy(GetSortProperty(request.SortColumn));

                IQueryable<ProductResponseList> productsResult = _mapper.Map<IQueryable<ProductResponseList>>(productsQuery);
                var products = await PagedResult<ProductResponseList>.CreateAsync(productsResult,
                    request.PageIndex,
                    request.PageSize);

                
                return products;
            }
        }
        public static Expression<Func<Domain.Entities.Product, object>> GetSortProperty(string? sortColumn)
            => sortColumn?.ToLower().Trim() switch
            {
                "name" => product => product.Name,
                "price" => product => product.Price,
                "description" => product => product.Description,
                _ => product => product.Id
            };
    }
}
