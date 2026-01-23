using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ArmsServices.DataServices;
using ArmsModels.BaseModels;

namespace Views.Data
{
    public class CustomAuthenticationSatetProvider : AuthenticationStateProvider
    {
        private ISessionStorageService _storage;
        private IUserService _userService;

        public CustomAuthenticationSatetProvider(ISessionStorageService storage, IUserService userService)
        {
            this._storage = storage;
            this._userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userID = await _storage.GetItemAsync<string>("userID");
                var identity = new ClaimsIdentity();

                if (userID != null)
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userID)
                    };

                    // Define a dummy ClaimsPrincipal to use for fetching roles (avoid infinite loop if service checks auth)
                    // Or better, just fetch by string userID directly if possible.
                    // Assuming GetCurrentUserBranchRole accepts just userID or we can look it up.
                    // Looking at UserService.cs will determine the exact method to call.
                    // For now, I'll attempt to fetch the role.
                    
                    var currentRole = _userService.GetCurrentBranchRole(userID); 
                    if (currentRole != null && currentRole.Branch != null)
                    {
                         claims.Add(new System.Security.Claims.Claim("BranchID", currentRole.Branch.BranchID.ToString()));
                         if (!string.IsNullOrEmpty(currentRole.Branch.BranchName))
                         {
                            claims.Add(new System.Security.Claims.Claim("BranchName", currentRole.Branch.BranchName));
                         }
                         if(currentRole.Role != null && !string.IsNullOrEmpty(currentRole.Role.RoleDesc))
                         {
                             claims.Add(new System.Security.Claims.Claim(ClaimTypes.Role, currentRole.Role.RoleDesc));
                         }
                    }

                    identity = new ClaimsIdentity(claims, "apiauth_type");
                } 
                var user = new ClaimsPrincipal(identity);
                return await Task.FromResult(new AuthenticationState(user));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }
        }

        public void AuthenticateUser(string userID)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userID)
            };

            var currentRole = _userService.GetCurrentBranchRole(userID); 
            if (currentRole != null && currentRole.Branch != null)
            {
                 claims.Add(new System.Security.Claims.Claim("BranchID", currentRole.Branch.BranchID.ToString()));
                 if (!string.IsNullOrEmpty(currentRole.Branch.BranchName))
                 {
                    claims.Add(new System.Security.Claims.Claim("BranchName", currentRole.Branch.BranchName));
                 }
                 if(currentRole.Role != null && !string.IsNullOrEmpty(currentRole.Role.RoleDesc))
                 {
                     claims.Add(new System.Security.Claims.Claim(ClaimTypes.Role, currentRole.Role.RoleDesc));
                 }
            }

            var identity = new ClaimsIdentity(claims, "apiauth_type");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
