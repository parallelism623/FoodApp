using AutoMapper;
using FoodShop.Application.Services.Product;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Contract.DataTransferObjects.Response.V1;
using FoodShop.Domain.Abstraction.Repositories;
using FoodShop.Domain.Exceptions;

namespace FoodShop.Application.UseCases.V1.Queries.ProductQuery
{
    public class GetProductByIdHandler : IQueryHandler<Query.GetProductByIdQuery, ProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public GetProductByIdHandler(IProductRepository productRepository, IMapper mapper)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<Result<ProductResponse>> Handle(Query.GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByIdAsync(request.Id) 
                          ?? throw new NotFoundException($"Product not found by Id: {request.Id}");
            var result = _mapper.Map<ProductResponse>(product);
            return result;
        }
    }
}
