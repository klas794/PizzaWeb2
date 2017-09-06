using InMemDbPizza.Data;
using InMemDbPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Services
{
    public class IngredientService
    {
        private readonly ApplicationDbContext _context;

        public IngredientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Ingredient> All()
        {
            return _context.Ingredient.OrderBy(i => i.Name).ToList();
        }
    }
}
