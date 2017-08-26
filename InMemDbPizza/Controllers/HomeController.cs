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

namespace InMemDbPizza.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public Cart GetCart()
        {
            var cartId = HttpContext.Session.Get<int?>("cartid");
            Cart cart = null;

            if (cartId == null)
            {
                cart = _context.Cart
                    .Include(x => x.CartItems)
                    .SingleOrDefault();

                //cart = new Cart();
                //cart = _context.Add(cart);
                //_context.SaveChanges();

                HttpContext.Session.Set<int?>("cartid", cart.CartId);
            }
            else
            {
                cart = _context.Cart
                    .Include(x => x.CartItems)
                    .SingleOrDefault(x => x.CartId == cartId);
            }

            if (cart == null)
            {
                cart = new Cart();
                HttpContext.Session.Set<int?>("cartid", cart.CartId);
            }

            return cart;
        }

        public IActionResult Index(int? categoryId)
        {
            var model = new MenuViewModel();

            model.Cart = GetCart();

            if(categoryId != null)
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
