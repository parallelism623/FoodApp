
using FluentValidation;
using FoodShop.Application.Common.DataTransferObjects.Request.V1;

namespace FoodShop.Application.Products.Validation
{
    public class CreateProductValidation : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidation()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ImageLink).NotEmpty();
            RuleFor(x => x.VideoLink).NotEmpty();
            RuleFor(x => x.ImageList).NotEmpty();
            RuleFor(x => x.DiscountPercent).LessThanOrEqualTo(100);
            RuleFor(x => x.View).Equal(0);

        }
    }
}
