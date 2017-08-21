using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDbPizza.Models
{
    public class DishIngredient
    {
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
