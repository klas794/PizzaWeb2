﻿using ProjectPizzaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDbPizza.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
        public List<CartItemIngredient> CartItemIngredients { get; set; }
    }
}
