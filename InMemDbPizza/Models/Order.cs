using InMemDbPizza.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public Payment Payment { get; set; }
        public int PaymentId { get; set; }

        public Cart Cart { get; set; }
        public int CartId { get; set; }

        public Address Address { get; set; }
        public int AddressId { get; set; }

        [Display(Name = "Time of order")]
        public DateTime OrderTime { get; set; }
    }
}
