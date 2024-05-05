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
    public record DeleteOrderCommand(Guid Id) : ICommand<string>;


    public class DeleteOrderHandler : ICommandHandler<DeleteOrderCommand, string>
    {
        private readonly IQueryRepository _queryRepository;
        private readonly ICommandRepository _commandRepository;
        public DeleteOrderHandler(IQueryRepository queryRepository, ICommandRepository commandRepository)
        {
            _queryRepository = queryRepository;
            _commandRepository = commandRepository;
        }

        public async Task<Result<string>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderQueryString = $"SELECT * FROM {nameof(Order)} WHERE Id = {request.Id}";
            var order = await _queryRepository.QuerySingleAsync<Order>(orderQueryString) ??
                throw new NotFoundException(MessageOrderResult.NotFound(request.Id));
            _commandRepository.Delete(order);
            return MessageOrderResult.DeleteSuccess(request.Id);
        }
    }
}
