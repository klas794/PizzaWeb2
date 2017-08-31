using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public Payment Payment { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
