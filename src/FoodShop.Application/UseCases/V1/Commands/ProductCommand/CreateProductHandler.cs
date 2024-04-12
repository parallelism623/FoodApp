using AutoMapper;
using FoodShop.Application.Services.Product;
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
using System.Windows.Input;

namespace FoodShop.Application.UseCases.V1.Commands.ProductCommand
{
    public class CreateProductHandler : ICommandHandler<CreateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public CreateProductHandler(IProductRepository productRepository, IMapper mapper) 
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByIdAsync(request.CreateProductRequest.Id);
            if (product != null)
                throw new BadRequestException($"Product found by id: {request.CreateProductRequest.Id}");
            var newProduct = _mapper.Map<Product>(request.CreateProductRequest);
            _productRepository.Add(newProduct);
            return Result.Success();
        }
    }
}
