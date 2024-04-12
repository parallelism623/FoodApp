using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        public DomainException(string title, string message) : base(message)
        {
            Title = title;
        }
        public string Title { get; }
    }
}
