using FoodShop.Contract.Abstraction.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Message
{
    public interface ICommandHandler<TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : ICommand
    {}
    public interface ICommandHandler<TRequest, TRespone> : IRequestHandler<TRequest, Result<TRespone>>
    where TRequest : ICommand<TRespone>
    { }
}
