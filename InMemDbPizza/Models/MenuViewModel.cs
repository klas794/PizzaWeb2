using InMemDbPizza.Models;
using ProjectPizzaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDbPizza.Models
{
    public class MenuViewModel
    {
        public List<Dish> Dishes { get; set; }
        public List<Category> Categories { get; set; }
        public Category Category { get; set; }
    }

}
