using Microsoft.AspNetCore.Mvc;
using ProjectPizzaWeb.Models;
using ProjectPizzaWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private CartService _cartService;
        private readonly CartItemService _cartItemService;

        public CartViewComponent(CartService cartService, CartItemService cartItemService)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new CartViewModel();

            model.Cart = await _cartService.GetCart(HttpContext.Session, HttpContext.User);

            model.ExtraIngredientsCount = _cartItemService.SumExtraIngredients(model.Cart.CartItems);

            return View(model);
        }
    }
}
