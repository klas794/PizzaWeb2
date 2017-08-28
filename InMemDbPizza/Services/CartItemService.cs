using InMemDbPizza.Data;
using Microsoft.EntityFrameworkCore;
using ProjectPizzaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Services
{
    public class CartItemService
    {
        private readonly ApplicationDbContext _context;

        public CartItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CartItemIngredient> GetIngredients(int cartItemId)
        {
            return _context.CartItemIngredients
                .Include(x => x.Ingredient)
                .Where(x => x.CartItemId == cartItemId).ToList();
        }
    }
}
