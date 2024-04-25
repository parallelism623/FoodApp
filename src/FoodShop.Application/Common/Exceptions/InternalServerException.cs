using FoodShop.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.Exceptions
{
    public class InternalServerException : DomainException
    {
        public InternalServerException(string message) : base("Server error", message)
        {
        }
    }
}
