using InMemDbPizza.Data;
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
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CartService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Cart> GetCart(ISession session, ClaimsPrincipal user)
        {
            var userId = _userManager.GetUserId(user);

            var appUser = await _userManager.GetUserAsync(user);

            var cartId = session.Get<int?>("cartid");

            Cart cart = cartId == null ? null: 
                _context.Cart
                    .Include(x => x.CartItems)
                    .ThenInclude(x => x.Dish)
                    .SingleOrDefault( x => x.CartId == cartId);

            if (cart == null)
            {
                cart = await this.CreateCart(session, user);
            }

            if (userId != cart.ApplicationUserId)
            {
                cart.ApplicationUserId = userId;
            }

            if(appUser != null ) { 
                cart.ApplicationUser = appUser;
            }
            
            return cart;
        }

        public async Task<Cart> CreateCart(ISession session, ClaimsPrincipal user)
        {

            var cart = new Cart()
            {
                CartItems = new List<CartItem>()
            };

            var userId = _userManager.GetUserId(user);

            if(userId != null && _signInManager.IsSignedIn(user))
            {
                cart.ApplicationUserId = userId;
            }

            var appUser = await _userManager.GetUserAsync(user);
            
            if(appUser != null && _signInManager.IsSignedIn(user))
            {
                cart.ApplicationUser = appUser;
            }

            await _context.AddAsync(cart);
            await _context.SaveChangesAsync();

            session.Set<int?>("cartid", cart.CartId);

            return cart;
        }
       
    }
}
