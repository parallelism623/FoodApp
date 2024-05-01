using AutoMapper;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities;
using FoodShop.Domain.Exceptions;

namespace FoodShop.Application.Products.ProductCommand
{
    public class UpdateProductHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        public UpdateProductHandler(
            ICommandRepository commandRepository,
            IQueryRepository queryRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var findIdSql = $"SELECT * FROM Product WHERE Id = {request.Id}";
            var product = await _queryRepository.QuerySingleAsync<FoodShop.Domain.Entities.Product>(findIdSql) ??
            throw new NotFoundException($"Product not found by id: {request.Id}");
            var newProduct = _mapper.Map<FoodShop.Domain.Entities.Product>(request.UpdateProductRequest);
            _commandRepository.Update(newProduct);
            return Result.Success();
        }
    }
}
