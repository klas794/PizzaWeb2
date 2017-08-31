using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InMemDbPizza.Models;
using ProjectPizzaWeb.Models;

namespace InMemDbPizza.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DishIngredient>()
                .HasKey(di => new { di.DishId, di.IngredientId });

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Dish)
                .WithMany(d => d.DishIngredients)
                .HasForeignKey(di => di.DishId);

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Ingredient)
                .WithMany(i => i.DishIngredients)
                .HasForeignKey(di => di.IngredientId);

            builder.Entity<Dish>()
                .HasOne(d => d.Category)
                .WithMany(c => c.Dishes)
                .HasForeignKey(d => d.CategoryId);

            //builder.Entity<CartItem>()
            //    .HasKey(ci => new { ci.CartItemId, ci.DishId, ci.CartId });

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Dish)
                .WithMany(d => d.CartItems)
                .HasForeignKey(ci => ci.DishId);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId);

            builder.Entity<CartItemIngredient>()
                .HasKey(cii => new { cii.CartItemId, cii.IngredientId/*, cii.CartId*/ });

            builder.Entity<CartItemIngredient>()
                .HasOne(cii => cii.CartItem)
                .WithMany(ci => ci.CartItemIngredients)
                .HasForeignKey(cii => cii.CartItemId);

            builder.Entity<CartItemIngredient>()
                .HasOne(cii => cii.Ingredient)
                .WithMany(i => i.CartItemIngredients)
                .HasForeignKey(cii => cii.IngredientId);


            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartItemIngredient> CartItemIngredients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PaymentChoice> PaymentChoices { get; set; }

    }
}
