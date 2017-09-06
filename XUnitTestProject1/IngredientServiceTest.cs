using InMemDbPizza.Data;
using InMemDbPizza.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPizzaWeb.Services;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class IngredientServiceTest : BaseTest
    {
        
        public override void InitializeDatabase()
        {
            base.InitializeDatabase();
            var context = _serviceProvider.GetService<ApplicationDbContext>();
            context.Ingredient.Add(new Ingredient { Name = "Nötter", Price = 1 });
            context.SaveChanges();
        }

        [Fact]
        public void All_Are_Sorted()
        {
            var _ingredients = _serviceProvider.GetService<IngredientService>();
            var ingsAll = _ingredients.All();

            Assert.Equal(1, ingsAll.Count);
        }
    }
}
