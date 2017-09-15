using InMemDbPizza.Data;
using InMemDbPizza.Models;
using InMemDbPizza.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectPizzaWeb.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddTransient<IngredientService>();
            services.AddTransient<CartItemService>();
            services.AddTransient<CartService>();
            services.AddTransient<LocalEmailSenderService>();

            _serviceProvider = services.BuildServiceProvider();

            InitializeDatabase();

        }

        public virtual void InitializeDatabase()
        {

        }

        public virtual async Task InitializeDatabaseAsync()
        {

        }

    }
}
