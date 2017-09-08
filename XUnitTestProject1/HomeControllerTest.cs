using InMemDbPizza.Controllers;
using InMemDbPizza.Data;
using InMemDbPizza.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectPizzaWeb.Models;
using System.Linq;
using Xunit;

namespace XUnitTestProject1
{
    public class HomeControllerTest : BaseTest
    {
        private string _selectedCategoryName;

        public override void InitializeDatabase()
        {
            base.InitializeDatabase();
            var context = _serviceProvider.GetService<ApplicationDbContext>();

            var category1 = new Category() { Name = "Pasta" };
            var category2 = new Category() { Name = "Salad" };

            _selectedCategoryName = category1.Name;

            var dish = new Dish { Name = "Spaghetti", Price = 50, Category = category1 };
            var dish2 = new Dish { Name = "Mixed salad", Price = 30, Category = category2 };
            var dish3 = new Dish { Name = "Lasagne", Price = 150, Category = category1 };

            context.AddRange(category1, category2);
            context.AddRange(dish, dish2, dish3);

            context.SaveChanges();
        }

        [Fact]
        public void Dishes_Are_Categorized()
        {
            // Arrange
            var dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = _serviceProvider.GetRequiredService<ILogger<HomeController>>();
            
            var controller = new HomeController(dbContext, userManager, logger);

            var categoryId = dbContext.Category
                .SingleOrDefault(x => x.Name == _selectedCategoryName).CategoryId;

            // Action
            var result = controller.Index(categoryId: categoryId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);

            Assert.NotNull(viewResult.ViewData);
            Assert.NotNull(viewResult.ViewData.Model);

            var model = Assert.IsType<MenuViewModel>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Categories.Count);
            Assert.Equal(2, model.Dishes.Count);
        }
    }
}
