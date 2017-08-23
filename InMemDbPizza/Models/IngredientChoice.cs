using InMemDbPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Models
{
    public class IngredientChoice
    {
        public Ingredient Ingredient { get; set; }
        public bool Checked { get; set; }
    }
}
