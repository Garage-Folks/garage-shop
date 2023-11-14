﻿using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace FineWoodworkingBasic.Authentication.Provider
{
    public class ApplicationUser : IdentityUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string NormalizedUserName { get; set; }
    }
}
