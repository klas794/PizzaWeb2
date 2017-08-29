using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InMemDbPizza.Models;
using InMemDbPizza.Data;
using ProjectPizzaWeb.Models;
using ProjectPizzaWeb.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProjectPizzaWeb.Services;

namespace InMemDbPizza.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CartService _cartService;
        private readonly CartItemService _cartItemService;

        public HomeController(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            CartService cartService,
            CartItemService cartItemService
            )
        {
            _context = context;
            _userManager = userManager;
            _cartService = cartService;
            _cartItemService = cartItemService;
        }
        
        public async Task<IActionResult> Index(int? categoryId)
        {
            var model = new MenuViewModel();

            model.Cart = await _cartService.GetCart(HttpContext.Session, User); // GetCart();

            model.ExtraIngredientsCount = _cartItemService.SumExtraIngredients(model.Cart.CartItems);
            
            if (categoryId != null)
            {
                model.Dishes = _context.Dishes.Where(x => x.CategoryId == categoryId).ToList();
                model.Category = _context.Category.SingleOrDefault(x => x.CategoryId == categoryId);
            }
            else
            {
                model.Dishes = _context.Dishes.ToList();
            }

            model.Categories = _context.Category.ToList();

            return View(model);
        }
        
        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
