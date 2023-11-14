using System.Security.Claims;

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
    }
}
