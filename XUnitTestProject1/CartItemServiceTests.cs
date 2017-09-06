using InMemDbPizza.Data;
using InMemDbPizza.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPizzaWeb.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject1
{
    public class CartItemServiceTests : BaseTest
    {
        public override void InitializeDatabase()
        {
            base.InitializeDatabase();
            var context = _serviceProvider.GetService<ApplicationDbContext>();

            var dish = new Dish { Name = "Spaghetti", Price = 50 };
            var ingredient = new Ingredient { Name = "Ketchup", Price = 50 };
            var dishIngredient = new DishIngredient { Dish = dish, Ingredient = ingredient };

            var extraIngredient = new Ingredient { Name = "Raw egg", Price = 10 };

            var dish2 = new Dish { Name = "Mixed salad", Price = 30 };
            var ingredient2 = new Ingredient { Name = "Onion", Price = 15 };
            var dishIngredient2 = new DishIngredient { Dish = dish2, Ingredient = ingredient2 };

            context.AddRange(ingredient, ingredient2);
            context.AddRange(dishIngredient, dishIngredient2);
            context.Add(extraIngredient);
            context.AddRange(dish, dish2);

            var cart = DbInitializer.SeedCartWithDish(dish, context, extraIngredient, quantity: 2);
            context.SaveChanges();

            DbInitializer.SeedCartWithDish(dish2, context, null, quantity: 1, cartTarget: cart);

            context.SaveChanges();
        }

        [Fact]
        public async Task Can_Calculate_Cart_Total_Price()
        {
            // Arrange
            var _cartItemService = _serviceProvider.GetService<CartItemService>();
            var _context = _serviceProvider.GetService<ApplicationDbContext>();

            var cart = await _context.Cart.SingleOrDefaultAsync();

            // Act
            var totals = _cartItemService.CalculateTotals( cart );

            // Assert
            Assert.Equal(150, totals);
        }
    }
}
