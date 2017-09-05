using ProjectPizzaWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDbPizza.Models
{
    public class Dish
    {
        public int DishId { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Range(1,int.MaxValue)]
        [Required]
        public int Price { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }

        [Display(Name = "Dish Ingredients")]
        public List<DishIngredient> DishIngredients { get; set; }
        public List<CartItem> CartItems { get; set; }

    }
}
