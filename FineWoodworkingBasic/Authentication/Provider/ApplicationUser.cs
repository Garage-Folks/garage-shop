using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace FineWoodworkingBasic.Authentication.Provider
{
    public class ApplicationUser : IdentityUser
    {
        public int? Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }

    }
}
