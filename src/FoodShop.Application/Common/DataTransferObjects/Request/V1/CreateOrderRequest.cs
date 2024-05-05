using FluentValidation;
using FoodShop.Contract.Abstraction.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Request.V1
{
    public class CreateOrderRequest
    {
        public Guid UserId { get; set; }
        public List<Guid> ProductsId { get; set; }
        public List<int> ProductsCount { get;set; }
    }
    public class CreateOrderRequestValidate : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidate() 
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id must be not null");
            RuleFor(x => x.ProductsId.Count).GreaterThan(0);
            RuleFor(x => x.ProductsId.Count).Equal(x => x.ProductsCount.Count());
        }

    
    }
}
