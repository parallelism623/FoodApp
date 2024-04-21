using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Domain.Exceptions
{
    public sealed class BadRequestException : DomainException
    {
        public BadRequestException(string title) : base(title, "BadRequest") { }

    }
}
