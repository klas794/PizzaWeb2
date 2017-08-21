using InMemDbPizza.Models;
using Microsoft.AspNetCore.Identity;
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
            var r = userManager.CreateAsync(aUser, "Pa$$word").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            if (context.Dishes.ToList().Count == 0)
            {
                var capricciosa = new Dish { Name = "Capricciosa", Price = 79 };
                var marg = new Dish { Name = "Marguerita", Price = 59 };
                var hawaii = new Dish { Name = "Hawaii", Price = 49 };

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

                context.AddRange(tomato, jalapeno, pineapple);
                context.AddRange(capricciosa, marg, hawaii);
                context.SaveChanges();
            }
        }
    }
}
