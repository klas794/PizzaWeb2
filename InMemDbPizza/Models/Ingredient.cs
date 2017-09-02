using ProjectPizzaWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDbPizza.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }

        [Required]
        public string Name { get; set; }

        public List<DishIngredient> DishIngredients { get; set; }
        public List<CartItemIngredient> CartItemIngredients { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Required]
        public int Price { get; internal set; } = 10;
    }
}
