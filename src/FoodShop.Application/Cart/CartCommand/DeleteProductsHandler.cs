using FoodShop.Application.Common.Auth;
using FoodShop.Application.Common.Repositories.Base;
using FoodShop.Contract.Abstraction.Message;
using FoodShop.Contract.Abstraction.Shared;
using FoodShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Cart.CartCommand
{
    public class DeleteProductsHandler : ICommandHandler<DeleteProductsCommand, string>
    {
        private readonly ICommandRepository _command;
        private readonly IQueryRepository _query;
        private readonly ICurrentUser _currentUser;
        public DeleteProductsHandler(
            ICommandRepository command,
            IQueryRepository query,
            ICurrentUser currentUser)
        {
            _command = command;
            _query = query;
            _currentUser = currentUser;
        }
        public async Task<Result<string>> Handle(DeleteProductsCommand request, CancellationToken cancellationToken)
        {
            var queryString = $"SELECT Id FROM Cart WHERE UserId = {_currentUser.GetUserId().ToString()}";
            var cartId = Guid.Parse(await _query.QuerySingleAsync<string>(queryString));
            _command.DeleteRange<CartProduct>(request.ProductsId.Select(x => new CartProduct
            {
                ProductId = x,
                CartId = cartId
            }));
            return "Delete Products success";
        }
    }
}
