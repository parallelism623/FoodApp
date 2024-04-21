using AutoMapper;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities;
using FoodShop.Domain.Exceptions;

namespace FoodShop.Application.Products.ProductQuery
{
    public class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, ProductResponse>
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        public GetProductByIdHandler(IQueryRepository queryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _queryRepository = queryRepository;
        }

        public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken = default)
        {
            var findByIdQuery = $"SELECT * FROM Product WHERE Id = {request.Id}";
            var product = await _queryRepository.QuerySingleAsync<Product>(findByIdQuery)
                          ?? throw new NotFoundException($"Product not found by Id: {request.Id}");
            var result = _mapper.Map<ProductResponse>(product);
            return result;
        }
    }
}
