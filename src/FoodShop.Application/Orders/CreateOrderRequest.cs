using AutoMapper;
using FoodShop.Application.Common.DataTransferObjects.Request.V1;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Constrant;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Abstraction;
using FoodShop.Domain.Entities;
using FoodShop.Domain.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Orders
{
    public record CreateOrderCommand(CreateOrderRequest Model) : ICommand<Guid>;
    public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, Guid>
    {
        private readonly IQueryRepository _queryRepostory;
        private readonly ICommandRepository _commandRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public CreateOrderHandler(
            IUnitOfWork unitOfWork,
            IQueryRepository queryRepository, 
            ICommandRepository commandRepository, 
            IMapper mapper)
        {
            _queryRepostory = queryRepository;
            _commandRepository = commandRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            List<string> guidStrings = model.ProductsId.Select(g => "'" + g.ToString() + "'").ToList();
            string guidList = string.Join(",", guidStrings);
            var productsQueryString = $"SELECT * FROM {nameof(Domain.Entities.Product)} " +
                $"WHERE {nameof(Domain.Entities.Product.Id)} IN " + guidList;
            var products = (await _queryRepostory.QueryAsync<Domain.Entities.Product>(productsQueryString)).ToList();
            int i = 0;
            foreach(var product in products)
            {
                if (product.Quantity < model.ProductsCount[i])
                    throw new BadRequestException(MessageProductResult.OutOfStock(product.Name));
                product.Quantity = model.ProductsCount[i];
            }                
            var amoutDetail = products.Aggregate(
                new { Price = 0m, Discount = 0m},
                (acc, p) => new
                {
                    Price = acc.Price + p.Price,
                    Discount = acc.Discount + p.Discount + p.DiscountPercent * acc.Price / 100,
                
                });
            
            var newOrder = new Order
            {
                AmoutTotal = amoutDetail.Price,
                Amout = amoutDetail.Price - amoutDetail.Discount,
                Discount = amoutDetail.Price,
                Tax = 0m,
                Title = "Order detail",
                Shipping = 20000m,

            };
            await _commandRepository.AddAsync<Order>(newOrder);
            await _unitOfWork.SaveChangesAsync();
            var orderProducts = _mapper.Map<List<OrderProduct>>(products);
            orderProducts.ForEach(x => x.OrderId = newOrder.Id);
            newOrder.OrderProducts = orderProducts;
            _commandRepository.Update<Order>(newOrder);
            return newOrder.Id;
        }
    }
}
