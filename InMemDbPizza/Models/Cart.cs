﻿using InMemDbPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
