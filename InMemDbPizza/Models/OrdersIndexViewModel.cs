using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Models
{
    public class OrdersIndexViewModel
    {
        public List<Order> Orders { get; set; }
        public int? DeliveredOrderId { get; set; }
        public Order DeliveredOrder { get; set; }
        public string ViewMode { get; set; }
    }
}
