using System.ComponentModel.DataAnnotations;

namespace InMemDbPizza.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        [Required]
        [Display(Name = "Full name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string PostalAddress { get; set; }

        [Required]
        [Display(Name = "Postal code")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:ddd dd}")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Phone #")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}