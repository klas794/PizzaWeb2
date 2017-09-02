using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Models
{
    public class ReviewOrderViewModel
    {
        public Cart Cart { get; set; }
        public List<PaymentChoice> PaymentChoices { get; set; }

        [Required]
        public PaymentChoice PaymentChoice { get; set; }
        
        [Required]
        [Display(Name = "Card #")]
        [CreditCard]
        public string CardNo { get; set; }

        [Required]
        [Display(Name = "CCV")]
        public int CCV { get; set; }

        [Required]
        [Display(Name = "Full name")]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Postal address")]
        public string PostalAddress { get; set; }

        [Required]
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        [Required]
        public string City { get; set; }
    }
}
