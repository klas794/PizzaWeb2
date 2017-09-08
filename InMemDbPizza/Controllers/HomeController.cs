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
using Microsoft.Extensions.Logging;

namespace InMemDbPizza.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            ILogger<HomeController> logger
            )
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        
        public IActionResult Index(int? categoryId)
        {
            var model = new MenuViewModel();

            if (categoryId != null)
            {
                model.Dishes = _context.Dishes
                    .Include(x => x.DishIngredients)
                    .ThenInclude(x => x.Ingredient)
                    .Where(x => x.CategoryId == categoryId)
                    .OrderBy(x => x.Name)
                    .ToList();
                model.Category = _context.Category.SingleOrDefault(x => x.CategoryId == categoryId);
            }
            else
            {
                model.Dishes = _context.Dishes
                    .Include(x => x.DishIngredients)
                    .ThenInclude(x => x.Ingredient)
                    .OrderBy(x => x.Name)
                    .ToList();
            }

            model.Categories = _context.Category
                .OrderBy(x => x.Name)
                .ToList();

            return View(model);
        }
        
        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
