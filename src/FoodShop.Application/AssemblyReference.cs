using System.Reflection;

namespace FoodShop.Application
{
    public class AssemblyReference
    {
        public static Assembly Assembly => typeof(AssemblyReference).Assembly;
    }
}
