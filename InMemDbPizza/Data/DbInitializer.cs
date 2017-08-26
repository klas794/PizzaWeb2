using InMemDbPizza.Models;
using Microsoft.AspNetCore.Identity;
using ProjectPizzaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemDbPizza.Data
{
    public class DbInitializer
    {
        public static void Initialize(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            var aUser = new ApplicationUser();
            aUser.UserName = "student@test.com";
            aUser.Email = "student@test.com";
            var r = userManager.CreateAsync(aUser, "Pa$$w0rd").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser();
            adminUser.UserName = "admin@test.com";
            adminUser.Email = "admin@test.com";

            var adminUserResult = userManager.CreateAsync(adminUser, "Pa$$w0rd");

            var roleAddedResult = userManager.AddToRoleAsync(adminUser, "Admin");

            if (context.Dishes.ToList().Count == 0)
            {
                var categoryGeneral = new Category { Name = "General" };
                var categoryBudget = new Category { Name = "Budget" };
                var capricciosa = new Dish { Category = categoryGeneral, Name = "Capricciosa", Price = 79 };
                var marg = new Dish { Category = categoryBudget, Name = "Marguerita", Price = 59 };
                var hawaii = new Dish { Category = categoryGeneral, Name = "Hawaii", Price = 49 };

                var cart = new Cart();
                var cartItem = new CartItem() { Cart = cart, Dish = marg, Quantity = 1 };

                cart.CartItems = new List<CartItem>();
                cart.CartItems.Add(cartItem);

                var tomato = new Ingredient { Name = "Tomato" };
                var jalapeno = new Ingredient { Name = "Jalapeno" };
                var pineapple = new Ingredient { Name = "Pineapple" };

                var capricciosaTomato = new DishIngredient { Dish = capricciosa, Ingredient = tomato };
                var margJalapeno = new DishIngredient { Dish = marg, Ingredient = jalapeno };
                var hawaiiPineapple = new DishIngredient { Dish = hawaii, Ingredient = pineapple };
                
                capricciosa.DishIngredients = new List<DishIngredient>();
                capricciosa.DishIngredients.Add(capricciosaTomato);
                marg.DishIngredients = new List<DishIngredient>();
                marg.DishIngredients.Add(margJalapeno);
                hawaii.DishIngredients = new List<DishIngredient>();
                hawaii.DishIngredients.Add(hawaiiPineapple);

                context.Add(cartItem);
                context.Add(cart);
                context.AddRange(tomato, jalapeno, pineapple);
                context.AddRange(categoryGeneral, categoryBudget);
                context.AddRange(capricciosa, marg, hawaii);
                context.SaveChanges();

            }
        }
    }
}
