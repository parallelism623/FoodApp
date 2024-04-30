using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Request.V1
{
    public class UpdateCategoryRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string MetaTitle { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
    }
    public class UpdateCategoryRequestValidation : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidation() 
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Category Id can't empty");
        }
    }
}
