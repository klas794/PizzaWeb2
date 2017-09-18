using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectPizzaWeb.Models;
using InMemDbPizza.Data;
using InMemDbPizza.Models;
using ProjectPizzaWeb.Services;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectPizzaWeb.Controllers
{
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartItemService _cartItemService;

        public StatsController(ApplicationDbContext context, CartItemService cartItemService)
        {
            _context = context;
            _cartItemService = cartItemService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = new StatsViewModel();

            model.CartsCreated = _context.Cart.Count();
            model.ConversionRatioCartsOrders = model.CartsCreated != 0 ?
                Math.Round(((decimal)_context.Orders.Count() / (decimal)model.CartsCreated ),2) * 100: 0;

            model.MostPopularDish = GetMostPopularDish();

            model.MostPopularIngredient = GetMostPopularIngredient();

            model.NumberOfDeliveredOrders = _context.Orders.Count(x => x.Delivered);
            model.NumberOfDeliveredDishes = _context.Orders.Include(x => x.Cart).ThenInclude(x => x.CartItems)
                .Where(x => x.Delivered)
                .Sum(x => x.Cart.CartItems.Sum(y => y.Quantity));

            model.OrdersThisWeek = _context.Orders.Count(x => x.OrderTime > DateTime.Today.AddDays(-7));
            model.OrdersToday = _context.Orders.Count(x => x.OrderTime > DateTime.Today);

            var lastOrderTime = _context.Orders.OrderBy(x => x.OrderTime).FirstOrDefault()?.OrderTime;

            if(lastOrderTime != null)
            {
                model.LastOrderTime = (DateTime)lastOrderTime;
            }

            model.NumberOfDishes = _context.Dishes.Count();
            model.NumberOfIngredients = _context.Ingredient.Count();

            var carts = _context.Cart.Include(x => x.CartItems).ThenInclude(x => x.Dish);

            if(carts.Count() > 0) {
                var cartsTotals = new List<int>();
                foreach (var item in carts)
                {
                    cartsTotals.Add(_cartItemService.CalculateTotals(item));
                }

                model.TotalIncome = cartsTotals.Sum();
                model.AverageIncome = (decimal)cartsTotals.Average();
            }

            return View(model);
        }

        private Dish GetMostPopularDish()
        {

            var orderedDishes = _context.Orders
                .Include(x => x.Cart)
                .ThenInclude(x => x.CartItems)
                .SelectMany(x => x.Cart.CartItems)
                .GroupBy(y => y.Dish.DishId)
                .Select(y => new { DishId = y.Key, Count = y.Sum(z => z.Quantity) });

            if(orderedDishes.Count() == 0)
            {
                return new Dish { Name = "N/A" };
            }

            var dishOrderMaximum = orderedDishes.Max(x => x.Count);

            var mostPopularDishId = orderedDishes
                        .FirstOrDefault(x => x.Count == dishOrderMaximum)?
                        .DishId;

            return _context.Dishes.Find(mostPopularDishId);
        }

        private Ingredient GetMostPopularIngredient()
        {
            
            var orderedIngredients = _context.CartItems
                .Include(x => x.CartItemIngredients)
                .SelectMany(x => x.CartItemIngredients)
                .GroupBy(y => y.Ingredient.IngredientId)
                .Select(y => new { IngredientId = y.Key, Count = y.Sum(z => z.CartItem.Quantity) });

            if (orderedIngredients.Count() == 0)
            {
                return new Ingredient { Name = "N/A" };
            }

            var ingredientOrderMaximum = orderedIngredients.Max(x => x.Count);

            var mostPopularIngredientId = orderedIngredients
                        .FirstOrDefault(x => x.Count == ingredientOrderMaximum)?
                        .IngredientId;

            return _context.Ingredient.Find(mostPopularIngredientId);
        }
    }
}
