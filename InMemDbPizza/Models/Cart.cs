using InMemDbPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Models
{
    public class Cart
    {
        public Cart()
        {
            SessionUserId = Guid.NewGuid();
        }

        public int CartId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ApplicationUserId { get; set; }
        public Guid SessionUserId { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
