using InMemDbPizza.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectPizzaWeb.Models;
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

        public List<IngredientChoice> IngredientsChoices { get; set; }
        
        public List<SelectListItem> Categories { get; set; }
    }
}