using InMemDbPizza.Data;
using InMemDbPizza.Models;
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
                    _context.DishIngredients
                        .Where(d => d.DishId == item.DishId)
                        .Count();

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

        public int SumExtraIngredientsValue(CartItem cartItem)
        {
            var originalDishIngredients =
                _context.DishIngredients
                    .Include(x => x.Ingredient)
                    .Where(x => x.DishId == cartItem.DishId)
                    .Select(x => x.Ingredient).ToList();

            var modifiedDishIngredients = this.GetIngredients(cartItem.CartItemId).Select(x => x.Ingredient).ToList();

            //var extraIngredients = modifiedDishIngredients.Except(originalDishIngredients);

            var extraIngredients = this.GetIngredients(cartItem.CartItemId)
                .Where(x => NotInOriginalDish(cartItem.DishId , x.IngredientId));
            
            return extraIngredients.Sum(x => x.Ingredient.Price);

        }

        private bool NotInOriginalDish(int dishId, int ingredientId)
        {
            return !_context.DishIngredients.Any(x => x.DishId == dishId && x.IngredientId == ingredientId);
        }

        public bool IngredientIsExtra (int dishId, int ingredientId)
        {
            return !_context.DishIngredients
                .Any(x => x.DishId == dishId && x.IngredientId == ingredientId);
        }
    }
}
