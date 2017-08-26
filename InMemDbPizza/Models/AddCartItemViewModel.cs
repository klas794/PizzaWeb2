using InMemDbPizza.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectPizzaWeb.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InMemDbPizza.Models
{
    public class AddCartItemViewModel
    {
        public AddCartItemViewModel()
        {

        }

        public CartItem CartItem { get; set; }

        public List<IngredientChoice> IngredientsChoices { get; set; }
    }
}