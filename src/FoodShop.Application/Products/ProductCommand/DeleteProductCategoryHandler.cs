using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities;
using FoodShop.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Product.ProductCommand
{
    public class DeleteProductCategoryHandler : ICommandHandler<DeleteProductCategoryCommand, string>
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IQueryRepository _queryRepository;
        public DeleteProductCategoryHandler(ICommandRepository commandRepository, IQueryRepository queryRepository)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
        }

        public async Task<Result<string>> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentProduct = await _queryRepository.IsExists<FoodShop.Domain.Entities.Product>(request.ProductId)
                ?? throw new NotFoundException(MessengerDomainResult.NotFound<FoodShop.Domain.Entities.Product>(request.ProductId));
            var currentCategory = await _queryRepository.IsExists<FoodShop.Domain.Entities.Category>(request.CategoryId)
                ?? throw new NotFoundException(MessengerDomainResult.NotFound<FoodShop.Domain.Entities.Category>(request.CategoryId));
            var productCategory = new ProductCategory { CategoryId = request.CategoryId, ProductId = request.ProductId};
            _commandRepository.Delete<ProductCategory>(productCategory);
            return MessengerDomainResult.DeleteSuccess<ProductCategory>();
        }
    }
}
