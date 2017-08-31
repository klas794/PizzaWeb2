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
                var categoryExpensive = new Category { Name = "Delux" };

                var capricciosa = new Dish { Category = categoryGeneral, Name = "Capricciosa", Price = 79 };
                var marg = new Dish { Category = categoryBudget, Name = "Marguerita", Price = 49 };
                var hawaii = new Dish { Category = categoryGeneral, Name = "Hawaii", Price = 69 };
                var gudfadern = new Dish { Category = categoryExpensive, Name = "Gudfadern", Price = 99 };
                var diablo = new Dish { Category = categoryExpensive, Name = "Diablo", Price = 89 };

                var tomato = new Ingredient { Name = "Tomato", Price = 5 };
                var jalapeno = new Ingredient { Name = "Jalapeno" };
                var pineapple = new Ingredient { Name = "Pineapple", Price = 15 };
                var ham = new Ingredient { Name = "Ham", Price = 8 };
                var onion = new Ingredient { Name = "Onion", Price = 11 };
                var blackPepper = new Ingredient { Name = "Black pepper", Price = 5 };
                var cheese = new Ingredient { Name = "Cheese", Price = 7 };
                var mushroom = new Ingredient { Name = "Mushroom", Price = 1 };
                var bea = new Ingredient { Name = "Bearnaise", Price = 30 };
                var chicken = new Ingredient { Name = "Chicken", Price = 18 };

                var capricciosaTomato = new DishIngredient { Dish = capricciosa, Ingredient = tomato };
                var capricciosaMushroom = new DishIngredient { Dish = capricciosa, Ingredient = mushroom };
                var margTomato = new DishIngredient { Dish = marg, Ingredient = tomato };
                var margCheese = new DishIngredient { Dish = marg, Ingredient = cheese };
                var hawaiiPineapple = new DishIngredient { Dish = hawaii, Ingredient = pineapple };
                var gudfadernHam = new DishIngredient { Dish = gudfadern, Ingredient = ham };
                var gudfadernOnion = new DishIngredient { Dish = gudfadern, Ingredient = onion };
                var gudfadernBlackPepper = new DishIngredient { Dish = gudfadern, Ingredient = blackPepper };
                var diabloBea = new DishIngredient { Dish = diablo, Ingredient = bea };
                var diabloChicken = new DishIngredient { Dish = diablo, Ingredient = chicken };
                var diabloJalapeno = new DishIngredient { Dish = diablo, Ingredient = jalapeno };
                
                capricciosa.DishIngredients = new List<DishIngredient>();
                capricciosa.DishIngredients.Add(capricciosaTomato);
                capricciosa.DishIngredients.Add(capricciosaMushroom);
                marg.DishIngredients = new List<DishIngredient>();
                marg.DishIngredients.Add(margTomato);
                marg.DishIngredients.Add(margCheese);
                hawaii.DishIngredients = new List<DishIngredient>();
                hawaii.DishIngredients.Add(hawaiiPineapple);
                gudfadern.DishIngredients = new List<DishIngredient>();
                gudfadern.DishIngredients.Add(gudfadernHam);
                gudfadern.DishIngredients.Add(gudfadernOnion);
                gudfadern.DishIngredients.Add(gudfadernBlackPepper);
                diablo.DishIngredients = new List<DishIngredient>();
                diablo.DishIngredients.Add(diabloBea);
                diablo.DishIngredients.Add(diabloChicken);
                diablo.DishIngredients.Add(diabloJalapeno);

                context.AddRange(tomato, jalapeno, pineapple, ham, onion, blackPepper, mushroom, bea, chicken);
                context.AddRange(categoryGeneral, categoryBudget, categoryExpensive);
                context.AddRange(capricciosa, marg, hawaii, gudfadern, diablo);

                // DbInitializer.AddSeededCart(marg, context);

                context.SaveChanges();

            }

            
        }

        public static void AddSeededCart(Dish dish, ApplicationDbContext context)
        {

            var cart = new Cart();
            var cartItem = new CartItem() { Cart = cart, Dish = dish, Quantity = 1 };
            cartItem.CartItemIngredients = new List<CartItemIngredient>();

            var cartItemIngredients = dish.DishIngredients.Select(x => new CartItemIngredient
            {
                CartItem = cartItem,
                Ingredient = x.Ingredient
            });

            cartItem.CartItemIngredients.AddRange(cartItemIngredients);

            cart.CartItems = new List<CartItem>();
            cart.CartItems.Add(cartItem);

            context.Add(cartItem);
            context.Add(cart);
        }
    }
}
