using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InMemDbPizza.Data;
using InMemDbPizza.Models;
using InMemDbPizza.Services;
using ProjectPizzaWeb.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using ProjectPizzaWeb.Models;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;

namespace InMemDbPizza
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            if (_environment.IsProduction() || _environment.IsStaging())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("DefaultConnection"));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<UserManager<ApplicationUser>>();
            services.AddTransient<CartService>();
            services.AddTransient<CartItemService>();
            services.AddTransient<LocalEmailSenderService>();

            services.AddMvc();

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, ILoggerFactory loggerFactory)
        {
            var svSECulture= "sv-SE";

            var supportedCultures = new[]
            {
                new CultureInfo(svSECulture),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(svSECulture),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            ConfigureAsync(userManager, context, roleManager, env).Wait();
        }

        public async Task ConfigureAsync(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IHostingEnvironment env)
        {
            if (env.IsProduction() || env.IsStaging())
            {
                await context.Database.MigrateAsync();
            }

            await DbInitializer.InitializeAsync(userManager, context, roleManager);
        }
    }
}
