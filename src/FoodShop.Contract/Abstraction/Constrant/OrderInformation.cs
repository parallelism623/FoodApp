using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Constrant
{
    public static class OrderInformation
    {
        enum TaxValueDefine
        {
            // Category - Tax percent
            Electronics = 1,
            Furnites = 5,
        }
        public static int GetTaxValue(string typeName)
            => typeName.ToLower() switch
            {
                "electrics" => (int)TaxValueDefine.Electronics,
                "furnites" => (int)TaxValueDefine.Furnites,
                _ => 0,
            };
        
    }
}
