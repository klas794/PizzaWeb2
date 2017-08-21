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
        public static void Initialize(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            var aUser = new ApplicationUser();
            aUser.UserName = "student@test.com";
            aUser.Email = "student@test.com";
            var r = userManager.CreateAsync(aUser, "Pa$$word").Result;

            if (context.Dishes.ToList().Count == 0)
            {
                var capricciosa = new Dish { Name = "Capricciosa", Price = 79 };

                var marg = new Dish { Name = "Marguerita", Price = 59 };

                var hawaii = new Dish { Name = "Hawaii", Price = 49 };

                context.AddRange(capricciosa, marg, hawaii);
                context.SaveChanges();
            }
        }
    }
}
