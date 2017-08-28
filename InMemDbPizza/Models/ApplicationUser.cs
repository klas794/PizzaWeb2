using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InMemDbPizza.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public  ApplicationUser()
        {
            UserId = Guid.NewGuid();
        }

        public Guid UserId { get; set; }


        [Required]
        public string PostalAddress { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Phone { get; set; }
    }

}
