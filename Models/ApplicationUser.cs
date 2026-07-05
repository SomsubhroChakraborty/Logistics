using Microsoft.AspNetCore.Identity;
using System;

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";
    }

