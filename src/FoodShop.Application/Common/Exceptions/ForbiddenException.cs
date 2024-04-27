using FoodShop.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.Exceptions
{
    public class ForbiddenException : DomainException
    {
        public ForbiddenException( string message) : base("Forbidden", message)
        {
        }
    }
}
