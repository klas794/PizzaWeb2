using InMemDbPizza.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPizzaWeb.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestProject1
{
    public class BaseTest
    {
        public readonly ServiceProvider _serviceProvider;

        public BaseTest()
        {
            var efServiceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(b =>
                b.UseInMemoryDatabase("Pizzadatabas")
                .UseInternalServiceProvider(efServiceProvider));

            services.AddTransient<IngredientService>();
            services.AddTransient<CartItemService>();

            _serviceProvider = services.BuildServiceProvider();

            InitializeDatabase();
        }

        public virtual void InitializeDatabase()
        {

        }

    }
}
