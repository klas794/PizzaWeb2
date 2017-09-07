using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Models
{
    public class CartViewModel
    {
        public Cart Cart { get; set; }
        public int ExtraIngredientsCount { get; set; }
    }
}
