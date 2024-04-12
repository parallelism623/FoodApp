using FoodShop.Domain.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Domain.Entities
{
    public class CartProduct
    {
        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }
        public int Quantity { get; set; }
        public bool IsSelected { get; set; }
        public decimal Price { get; set; }
        public string Context { get; set; }

    }
}
