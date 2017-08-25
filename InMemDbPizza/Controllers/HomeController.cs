using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InMemDbPizza.Models;
using InMemDbPizza.Data;
using ProjectPizzaWeb.Models;

namespace InMemDbPizza.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryId)
        {
            var model = new MenuViewModel();

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
