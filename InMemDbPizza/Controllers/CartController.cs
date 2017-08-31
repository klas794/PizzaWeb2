using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectPizzaWeb.Models;
using ProjectPizzaWeb.Extensions;
using InMemDbPizza.Data;
using InMemDbPizza.Models;
using InMemDbPizza.Controllers;
using Microsoft.EntityFrameworkCore;
using ProjectPizzaWeb.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectPizzaWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;

        public CartController(ApplicationDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CartAddToDish(string submit)
        {
            int dishId;
            string action = submit.Substring(0, submit.IndexOf('-'));
            int.TryParse(submit.Substring(submit.IndexOf('-') + 1), out dishId);

            return RedirectToAction(nameof(AddDish), new { dishId = dishId, act = action });
        }

        public async Task<IActionResult> AddDish(int dishId, string act)
        {
            var dish = _context.Dishes.SingleOrDefault(x => x.DishId == dishId);

            var dishIngredients = _context.DishIngredients
                .Include(x => x.Ingredient)
                .Where(x => x.DishId == dishId)
                .ToList();

            var cart = await _cartService.GetCart(HttpContext.Session, User);

            var cartItem = new CartItem() {
                DishId = dishId,
                Dish = dish,
                Cart = cart
            };

            if(act == "add")
            {
                cartItem.CartItemIngredients = dishIngredients.Select(x => new CartItemIngredient {
                        CartItemId = cartItem.CartItemId,
                        CartItem = cartItem,
                        Ingredient = x.Ingredient,
                        IngredientId = x.IngredientId
                    })
                    .ToList();
                _context.AddRange(cartItem.CartItemIngredients);
                _context.Add(cartItem);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index), "Home");
            }

            var model = new AddCartItemViewModel();

            model.IngredientsChoices =
                _context.Ingredient
                .Select(x => new IngredientChoice
                {
                    Ingredient = x,
                    Checked = dishIngredients.Any(y => y.Ingredient == x)
                })
                .OrderBy(x => x.Ingredient.Name)
                .ToList();

            model.CartItem = cartItem;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDish(AddCartItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cartItem = new CartItem()
                {
                    Dish = _context.Dishes.Find(model.CartItem.DishId),
                    Quantity = model.CartItem.Quantity
                };

                var cart = await _cartService.GetCart(HttpContext.Session, User);

                cart.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();

                cartItem.CartItemIngredients = new List<CartItemIngredient>();

                cartItem.CartItemIngredients = model.IngredientsChoices
                    .Where(x => x.Checked == true)
                    .Select(x => new CartItemIngredient {
                        CartItemId = cartItem.CartItemId,
                        CartItem = cartItem,
                        IngredientId = x.Ingredient.IngredientId,
                        Ingredient = _context.Ingredient.Find(x.Ingredient.IngredientId)
                    })
                    .ToList();

                _context.AddRange(cartItem.CartItemIngredients);
                _context.Update(cartItem);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), "Home");
            }
            return View(model);
        }

        public IActionResult EditCartItem(int cartItemId)
        {
            var model = new AddCartItemViewModel();

            var cartItem = _context.CartItems
                .Include(x => x.Dish)
                .SingleOrDefault(x => x.CartItemId == cartItemId);

            var cartItemIngredients = _context.CartItemIngredients
                .Include(x => x.Ingredient)
                .Where(x => x.CartItemId == cartItem.CartItemId)
                .ToList();

            model.IngredientsChoices =
                _context.Ingredient
                .Select(x => new IngredientChoice
                {
                    Ingredient = x,
                    Checked = cartItemIngredients
                        .Any(y => y.Ingredient.IngredientId == x.IngredientId && cartItemId == y.CartItemId)
                })
                .OrderBy(x => x.Ingredient.Name)
                .ToList();
            
            model.CartItem = cartItem;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCartItem(AddCartItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cartItem = _context.CartItems
                    .Include(x => x.CartItemIngredients)
                    .SingleOrDefault(x => x.CartItemId == model.CartItem.CartItemId);

                _context.RemoveRange(cartItem.CartItemIngredients);

                await _context.SaveChangesAsync();


                cartItem.Quantity = model.CartItem.Quantity;


                cartItem.CartItemIngredients = new List<CartItemIngredient>();

                cartItem.CartItemIngredients = model.IngredientsChoices
                    .Where(x => x.Checked == true)
                    .Select(x => new CartItemIngredient
                    {
                        CartItemId = cartItem.CartItemId,
                        CartItem = cartItem,
                        Ingredient = _context.Ingredient.SingleOrDefault(y => y.IngredientId == x.Ingredient.IngredientId),
                        IngredientId = x.Ingredient.IngredientId
                    })
                    .ToList();

                _context.Update(cartItem);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), "Home");
            }
            return View(model);
        }

        public IActionResult DeleteCartItem(int cartItemId)
        {
            var item = _context.CartItems.Find(cartItemId);

            _context.Remove(item);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> ReviewOrder(ReviewOrderViewModel model)
        {
            model = new ReviewOrderViewModel();
            
            model.Cart = await _cartService.GetCart(HttpContext.Session, HttpContext.User);

            model.PaymentChoices = await _context.PaymentChoices.ToListAsync();

            if(model.Cart.ApplicationUser != null)
            {
                model.Email = model.Cart.ApplicationUser.Email;
                model.PostalAddress = model.Cart.ApplicationUser.PostalAddress;
                model.PostalCode = model.Cart.ApplicationUser.PostalCode;
                model.City = model.Cart.ApplicationUser.City;
                model.PhoneNumber = model.Cart.ApplicationUser.PhoneNumber;
            }

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(ReviewOrderViewModel model)
        {
            var payment = new Payment
            {
                CardNo = model.CardNo,
                CardControlNumber = model.CardControlNumber
            };

            _context.Add(payment);
            await _context.SaveChangesAsync();

            var order = new Order
            {
                Payment = payment,
                CartItems = model.Cart.CartItems
            };

            _context.Add(order);
            await _context.SaveChangesAsync();

            await _cartService.CreateCart(HttpContext.Session, HttpContext.User);

            return RedirectToAction("OrderConfirmation", order);
        }

    }
}
