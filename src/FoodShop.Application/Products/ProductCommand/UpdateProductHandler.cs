using AutoMapper;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Abstraction.Repositories;
using FoodShop.Domain.Entities;
using FoodShop.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Products.ProductCommand
{
    public class UpdateProductHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public UpdateProductHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByIdAsync(request.Id) ??
                          throw new NotFoundException($"Product not found by id: {request.Id}");
            var newProduct = _mapper.Map<Product>(request.UpdateProductRequest);
            _productRepository.Update(newProduct);
            return Result.Success();
        }
    }
}
