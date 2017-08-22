using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InMemDbPizza.Models
{
    public class CreateDishViewModel
    {
        public CreateDishViewModel()
        {

        }

        public Dish Dish { get; set; }

        [Display(Name = "Select ingredients")]

        public List<Ingredient> AllIngredients { get; set; }
    }
}