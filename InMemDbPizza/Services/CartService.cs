﻿using InMemDbPizza.Data;
using InMemDbPizza.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectPizzaWeb.Extensions;
using ProjectPizzaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Cart> GetCart(ISession session, ClaimsPrincipal user)
        {
            var userIdResult = _userManager.GetUserId(user);
            int userId = 0;
            int.TryParse(userIdResult, out userId);

            var appUser = await _userManager.GetUserAsync(user);

            var cartId = session.Get<int?>("cartid");

            Cart cart = cartId == null ? null: 
                _context.Cart
                    .Include(x => x.CartItems)
                    .ThenInclude(x => x.Dish)
                    .SingleOrDefault( x => x.CartId == cartId);

            if (cart == null)
            {
                
                cart = new Cart() {
                    ApplicationUserId = userId,
                    CartItems = new List<CartItem>()
                };


                if (appUser != null)
                {
                    cart.ApplicationUser = appUser;
                }

                _context.Add(cart);
                _context.SaveChanges();

                session.Set<int?>("cartid", cart.CartId);
            }

            if(userId != cart.ApplicationUserId)
            {
                cart.ApplicationUserId = userId;
                cart.ApplicationUser = appUser;
            }

            return cart;
        }
    }
}
