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
        [Display(Name = "Payment #")]
        public int PaymentId { get; set; }

        public Cart Cart { get; set; }
        [Display(Name = "Cart #")]
        public int CartId { get; set; }

        public Address Address { get; set; }
        [Display(Name = "Address #")]
        public int AddressId { get; set; }

        [Display(Name = "Time of order")]
        public DateTime OrderTime { get; set; }
    }
}
