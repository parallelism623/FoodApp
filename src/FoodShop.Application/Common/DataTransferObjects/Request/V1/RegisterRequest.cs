using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Request.V1
{
    public class RegisterRequest
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Username => Email;

    } 
    public class RegisterRequestValidation : AbstractValidator<RegisterRequest>
    {   
        public RegisterRequestValidation()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty().WithMessage("Invalid email address");
            RuleFor(x => x.LastName).MaximumLength(20).NotEmpty();
            RuleFor(x => x.FirstName).MaximumLength(20).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(8).MaximumLength(20).NotEmpty();
            RuleFor(x => x.ConfirmPassword).MinimumLength(8).MaximumLength(20).NotEmpty().Equal(x => x.Password);
        }
    }
}
