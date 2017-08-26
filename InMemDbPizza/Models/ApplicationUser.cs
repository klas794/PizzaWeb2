﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

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
    }

}
