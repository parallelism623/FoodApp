using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Shared
{
    public record ValidationError(string PropertyName, string ErrorMessage);
}
