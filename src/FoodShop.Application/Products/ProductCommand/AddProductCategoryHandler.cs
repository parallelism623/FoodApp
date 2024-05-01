using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities;
using FoodShop.Domain.Exceptions;

namespace FoodShop.Application.Product.ProductCommand
{
    public class AddProductCategoryHandler : ICommandHandler<AddProductCategoryCommand, string>
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IQueryRepository _queryRespository;
        public AddProductCategoryHandler(ICommandRepository commandRepository, IQueryRepository queryRespository)
        {
            _commandRepository = commandRepository;
            _queryRespository = queryRespository;
        }

        public async Task<Result<string>> Handle(AddProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentProduct = await _queryRespository.IsExists<FoodShop.Domain.Entities.Product>(request.productId) ??
                throw new NotFoundException(MessengerDomainResult.NotFound<FoodShop.Domain.Entities.Product>(request.productId));
            var currentCategory = await _queryRespository.IsExists<FoodShop.Domain.Entities.Category>(request.categoryId) ??
                throw new NotFoundException(MessengerDomainResult.NotFound<FoodShop.Domain.Entities.Category>(request.categoryId));
            var categoryProduct = new ProductCategory{ CategoryId = request.categoryId, ProductId = request.productId };
            await _commandRepository.AddAsync(categoryProduct);

            return MessengerDomainResult.CreateSuccess<ProductCategory>();
        }
    }
}
