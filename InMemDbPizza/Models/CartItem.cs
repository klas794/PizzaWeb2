using InMemDbPizza.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        public Cart Cart { get; set; }
        public int CartId { get; set; }

        public Dish Dish { get; set; }
        public int DishId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        public List<CartItemIngredient> CartItemIngredients { get; set; }
    }
}
