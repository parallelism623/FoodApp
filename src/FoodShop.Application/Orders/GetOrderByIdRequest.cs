using AutoMapper;
using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
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

namespace FoodShop.Application.Orders
{
    public record GetOrderByIdRequest(Guid Id) : IQuery<OrderResponse>;


    public class GetOrderByIdHandler : IQueryHandler<GetOrderByIdRequest, OrderResponse>
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IMapper _mapper;
        public GetOrderByIdHandler(IQueryRepository queryRepository, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _mapper = mapper;
        }

        public async Task<Result<OrderResponse>> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
        {
            var orderSqlQueryString = $"SELECT * FROM {nameof(Order)} WHERE {nameof(Order.Id)} = {request.Id.ToString()}";
            var resultOrder = await _queryRepository.QueryAsync<Order>(orderSqlQueryString)
                ?? throw new NotFoundException(MessageOrderResult.NotFound(request.Id));
            var orderProductSqlQueryString
                = @$"SELECT P.* FROM {nameof(Domain.Entities.Product)} as P INNER JOIN 
                    (SELECT * FROM {nameof(OrderProduct)} WHERE {nameof(OrderProduct.OrderId)} = {request.Id}) as OP
                    WHERE P.{nameof(Domain.Entities.Product.Id)} = OP.{nameof(OrderProduct.ProductId)}";
            var resultProduct = (await _queryRepository.QueryAsync<Domain.Entities.Product>(orderProductSqlQueryString)).ToList();
            var result = _mapper.Map<OrderResponse>(resultOrder);
            result.Products = _mapper.Map<List<OrderProductRespone>>(resultProduct);
            return result;
        }
    }
}
