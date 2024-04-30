using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Request.V1
{
    public class CreateCategoryRequest
    {
        public Guid ParentId { get; set; }
        public string Title { get; set; }
        public string MetaTitle { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
    }
    public class CreateCategoryRequestValidation : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidation()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(30);
            RuleFor(x => x.MetaTitle).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Content).NotEmpty().MaximumLength(255);
        }
    }
}
