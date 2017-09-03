using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InMemDbPizza.Data;
using InMemDbPizza.Models;
using ProjectPizzaWeb.Models;

namespace InMemDbPizza.Controllers
{
    public class DishesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            return View(
                await _context.Dishes
                .Include(x => x.Category)
                .OrderBy(x => x.Name)
                .ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.DishId == id);

            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            var model = new CreateDishViewModel();

            model.Categories = _context.Category
                .Select(x => new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() })
                .OrderBy(x => x.Text)
                .ToList();

            model.IngredientsChoices = _context.Ingredient
                .Select(x => new IngredientChoice { Ingredient = x })
                .OrderBy(x => x.Ingredient.Name)
                .ToList();

            return View(model);
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDishViewModel model)
        {
            var ingredientsSelected = model.IngredientsChoices.Count(x => x.Checked);

            if (ingredientsSelected == 0)
            {
                ModelState.AddModelError("IngredientsChoices", "Select at least one ingredient");
            }

            if (ModelState.IsValid)
            {
                var dish = new Dish
                {
                    Name = model.Dish.Name,
                    Price = model.Dish.Price,
                    CategoryId = model.Dish.CategoryId,
                    DishIngredients = new List<DishIngredient>()
                };

                _context.Add(dish);
                await _context.SaveChangesAsync();

                foreach (var choice in model.IngredientsChoices)
                {
                    if(choice.Checked)
                    {
                        var dishIngredient = new DishIngredient {
                            Dish = dish,
                            Ingredient = _context.Ingredient.SingleOrDefault( x => 
                                x.IngredientId == choice.Ingredient.IngredientId ),
                        };

                        _context.Add(dishIngredient);
                        await _context.SaveChangesAsync();

                        dish.DishIngredients.Add(dishIngredient);
                    }
                }

                _context.Update(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.Categories = _context.Category
                .Select(x => new SelectListItem { Text = x.Name, Value = x.CategoryId.ToString() })
                .OrderBy(x => x.Text)
                .ToList();

            model.IngredientsChoices = _context.Ingredient
                .Select(x => new IngredientChoice { Ingredient = x })
                .OrderBy(x => x.Ingredient.Name)
                .ToList();

            return View(model);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = _context.Dishes.Find(id);
            
            if (dish == null)
            {
                return NotFound();
            }
            
            var model = new CreateDishViewModel();

            model.Dish = dish;

            var dishIngredients = await _context.DishIngredients
                .Where(x => x.DishId == dish.DishId)
                .ToListAsync();

            model.IngredientsChoices = await _context.Ingredient
                .Select(x => new IngredientChoice {
                    Ingredient = x,
                    Checked = dishIngredients.Any(y => y.Ingredient == x)
                })
                .OrderBy(x => x.Ingredient.Name)
                .ToListAsync();

            model.Categories = await _context.Category.Select(
                x => new SelectListItem {
                    Text = x.Name, Value = x.CategoryId.ToString(),
                    Selected = dish.CategoryId ==  x.CategoryId
                })
                .OrderBy(x => x.Text)
                .ToListAsync();

            return View(model);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateDishViewModel dishModel)
        {
            var ingredientsSelected = dishModel.IngredientsChoices.Count(x => x.Checked);

            if (ingredientsSelected == 0)
            {
                ModelState.AddModelError("IngredientsChoices", "Select at least one ingredient");
            }

            if (id != dishModel.Dish.DishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dishModel.Dish.DishIngredients = new List<DishIngredient>();
                    foreach (var choice in dishModel.IngredientsChoices)
                    {
                        var exists = _context.DishIngredients.Any(
                            x => x.DishId == dishModel.Dish.DishId && 
                            x.Ingredient.IngredientId == choice.Ingredient.IngredientId);

                        if (choice.Checked && !exists)
                        {
                            var dishIngredient = new DishIngredient
                            {
                                Dish = dishModel.Dish,
                                Ingredient = _context.Ingredient
                                    .SingleOrDefault(x => x.IngredientId == choice.Ingredient.IngredientId)
                            };

                            _context.Add(dishIngredient);
                            dishModel.Dish.DishIngredients.Add(dishIngredient);
                        }
                        else if(!choice.Checked && exists)
                        {
                            var dishIngredient = _context.DishIngredients.SingleOrDefault(
                                x => x.DishId == dishModel.Dish.DishId && 
                                x.Ingredient.IngredientId == choice.Ingredient.IngredientId
                                );

                            if (dishIngredient != null)
                            {
                                _context.Remove(dishIngredient);
                                dishModel.Dish.DishIngredients.Remove(dishIngredient);
                            }
                        }
                    }

                    _context.Update(dishModel.Dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dishModel.Dish.DishId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                dishModel.Categories = await _context.Category.Select(
                    x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.CategoryId.ToString(),
                        Selected = dishModel.Dish.CategoryId == x.CategoryId
                    })
                    .OrderBy(x => x.Text)
                    .ToListAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(dishModel);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.DishId == id);
        }
    }
}
