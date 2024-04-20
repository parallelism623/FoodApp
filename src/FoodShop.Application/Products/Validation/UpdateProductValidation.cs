using FluentValidation;
using FoodShop.Contract.DataTransferObjects.Request.V1;

namespace FoodShop.Application.Products.Validation
{
    public class UpdateProductValidation : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidation()
        {
            RuleFor(x => x.Description).MaximumLength(255);
            RuleFor(x => x.Name).MaximumLength(255);
            RuleFor(x => x.DiscountPercent).LessThanOrEqualTo(100);
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.View).GreaterThanOrEqualTo(0);
        }
    }
}
