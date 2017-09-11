using InMemDbPizza.Controllers;
using InMemDbPizza.Data;
using InMemDbPizza.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectPizzaWeb.Controllers;
using ProjectPizzaWeb.Models;
using ProjectPizzaWeb.Services;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject1
{
    public class CartControllerTest : BaseTest
    {
        private Mock<HttpContext> _httpContext;
        private Mock<ISession> _mockSession;
        private Mock<ClaimsPrincipal> _mockUser;
        private CartService _cartService;

        public override async Task InitializeDatabaseAsync()
        {
            await base.InitializeDatabaseAsync();
            var context = _serviceProvider.GetService<ApplicationDbContext>();

            var category1 = new Category() { Name = "Pasta" };
            var category2 = new Category() { Name = "Salad" };

            var dish = new Dish { Name = "Spaghetti", Price = 50, Category = category1 };
            var dish2 = new Dish { Name = "Mixed salad", Price = 30, Category = category2 };
            var dish3 = new Dish { Name = "Lasagne", Price = 150, Category = category1 };
            
            await context.AddRangeAsync(category1, category2);
            await context.AddRangeAsync(dish, dish2, dish3);

            await context.SaveChangesAsync();

            var paymentProvider = new PaymentChoice { Name = "Silver club" };

            await context.AddAsync(paymentProvider);
            await context.SaveChangesAsync();

            var cart = DbInitializer.SeedCartWithDish(dish, context, null, quantity: 2);
            cart = DbInitializer.SeedCartWithDish(dish2, context, null, quantity: 3, cartTarget: cart);
            cart = DbInitializer.SeedCartWithDish(dish3, context, null, quantity: 3, cartTarget: cart);
            await context.AddAsync(cart);
            await context.SaveChangesAsync();

            _httpContext = new Mock<HttpContext>();

            _mockSession = new Mock<ISession>();
            _mockUser = new Mock<ClaimsPrincipal>();

            _mockSession.Object.SetInt32("cartid", cart.CartId);

            _httpContext.Setup(s => s.Session).Returns(_mockSession.Object);
            _httpContext.Setup(s => s.User).Returns(_mockUser.Object);

            _cartService = _serviceProvider.GetRequiredService<CartService>();

            return;
        }

        [Fact]
        public async Task Order_Can_Be_PlacedAsync()
        {
            await InitializeDatabaseAsync();

            // Arrange

            var dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = _serviceProvider.GetRequiredService<ILogger<CartController>>();

            var actionDescriptor = new Mock<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>().Object;
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(_httpContext.Object, new Mock<RouteData>().Object, actionDescriptor, modelState);

            var controller = new CartController(dbContext, _cartService, logger);
            controller.ControllerContext = new ControllerContext(actionContext);

            var reviewOrderViewModel = new ReviewOrderViewModel()
            {
                CardNo = "123",
                CCV = "312",
                PaymentChoice = dbContext.PaymentChoices
                    .Select(x => new PaymentChoice {
                        Name = x.Name, PaymentChoiceId = x.PaymentChoiceId})
                    .SingleOrDefault(),
                Name = "Otto Ottosson",
                PostalAddress = "Ogatan 1",
                PostalCode = "12332",
                City = "Osala",
                Email = "otto@osala.se",
                PhoneNumber = "123"
            };

            reviewOrderViewModel.Cart = 
                await _cartService.GetCart(_mockSession.Object, _mockUser.Object);

            var preNumberOfOrders = dbContext.Orders.Count();

            // Action
            var result = await controller.PlaceOrder(reviewOrderViewModel);
            
            // Assert
            //var viewResult = Assert.IsType<IActionResult>(result);
            //Assert.Null(viewResult.ExecuteResultAsync().ViewName);
           
            //Assert.NotNull(viewResult.ViewData);
            //Assert.NotNull(viewResult.ViewData.Model);

            //var model = Assert.IsType<Order>(viewResult.ViewData.Model);

            var postNoOfOrders = dbContext.Orders.Count();

            Assert.Equal(preNumberOfOrders + 1, postNoOfOrders);
        }
    }
}
