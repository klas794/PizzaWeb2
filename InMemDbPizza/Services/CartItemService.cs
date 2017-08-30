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

        public int SumExtraIngredients(List<CartItem> cartItems)
        {
            var total = 0;

            foreach (var item in cartItems)
            {
                var originalDishIngredientsCount =
                    _context.Dishes
                        .Where(d => d.DishId == item.DishId)
                        .Select(d => d.DishIngredients).Count();

                var modifiedDishIngredientsCount = this.GetIngredients(item.CartItemId).Count();

                var extraIngredientsCount = modifiedDishIngredientsCount - originalDishIngredientsCount;

                total += extraIngredientsCount > 0 ? extraIngredientsCount * item.Quantity : 0;
            }

            return total;
        }

        public int SumExtraIngredientsForOne(CartItem cartItem)
        {
            var originalDishIngredientsCount =
                _context.Dishes
                    .Where(d => d.DishId == cartItem.DishId)
                    .Select(d => d.DishIngredients).Count();

            var modifiedDishIngredientsCount = this.GetIngredients(cartItem.CartItemId).Count();

            var extraIngredientsCount = modifiedDishIngredientsCount - originalDishIngredientsCount;

            return extraIngredientsCount > 0 ? extraIngredientsCount : 0;
        }

    }
}
