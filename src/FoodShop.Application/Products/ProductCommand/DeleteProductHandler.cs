using AutoMapper;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities;
using FoodShop.Domain.Exceptions;

namespace FoodShop.Application.Products.ProductCommand
{
    public class DeleteProductHandler : ICommandHandler<DeleteProductCommand>
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        public DeleteProductHandler(
            ICommandRepository commandRepository,
            IMapper mapper,
            IQueryRepository queryRepository)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
            _queryRepository = queryRepository;
        }
        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var findIdSql = $"SELECT * FROM Product WHERE Id = {request.Id}";
            var product = await _queryRepository.QuerySingleAsync<Product>(findIdSql);
            if (product != null)
                throw new NotFoundException($"Product not found by id: {request.Id}");
            await _commandRepository.AddAsync(product);
            return Result.Success();
        }
    }
}
