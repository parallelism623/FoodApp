using AutoMapper;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities;
using FoodShop.Domain.Exceptions;

namespace FoodShop.Application.Products.ProductCommand
{
    public class CreateProductHandler : ICommandHandler<CreateProductCommand>
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        public CreateProductHandler(
            ICommandRepository commandRepository, 
            IMapper mapper,
            IQueryRepository queryRepository)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
            _queryRepository = queryRepository;
        }
        public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var findIdSql = $"SELECT * FROM Product WHERE Id = {request.Id}";
            var product = await _queryRepository.QuerySingleAsync<Product>(findIdSql);
            if (product != null)
                throw new BadRequestException($"Product found by id: {request.Id}");
            var newProduct = _mapper.Map<Product>(request.CreateProductRequest);
            await _commandRepository.AddAsync(newProduct);
            return Result.Success();
        }
    }
}
