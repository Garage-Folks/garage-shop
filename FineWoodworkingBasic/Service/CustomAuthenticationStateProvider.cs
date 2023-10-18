using System.Security.Claims;
using FineWoodworkingBasic.Model;
using Microsoft.AspNetCore.Components.Authorization;

namespace FineWoodworkingBasic.Service
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly LoginService loginService;
        public CustomAuthenticationStateProvider(LoginService login)
        {
            loginService = login;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state = new AuthenticationState(new ClaimsPrincipal());

            bool isLoggedIn = loginService.isLoggedIn();
            if (isLoggedIn) 
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "User")
                }, "test authentication type");

                state = new AuthenticationState(new ClaimsPrincipal(identity));
            }

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
    }
}
