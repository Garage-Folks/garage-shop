using System.Security.Claims;
using System.Security.Principal;

namespace FineWoodworkingBasic.Authentication
{
    public class AuthenticationService
    {
        public event Action<ClaimsPrincipal>? UserChanged;
        private static ClaimsPrincipal? currentUser;

        public ClaimsPrincipal CurrentUser
        {
            get { return currentUser ?? new(); }
            set
            {
                currentUser = value;

                if (UserChanged is not null)
                {
                    UserChanged(currentUser);
                }
            }
        }

        public void SetUser(string username, string role)
        {
            var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Name, username), 
                new Claim(ClaimTypes.Role, role)
            },
            "Custom Authentication");

            currentUser = new ClaimsPrincipal(identity);
        }

        public void LogoutUser()
        {
            currentUser = new ClaimsPrincipal();
        }
    }
}
