using FoodShop.Contract.Abstraction.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Message
{
    public interface ICommand : IRequest<Result> { }
    public interface ICommand<TRespone> : IRequest<Result<TRespone>> { }
}
