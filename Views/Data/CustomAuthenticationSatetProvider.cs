using ArmsModels.BaseModels;
using ArmsServices.DataServices;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Views.Data
{
    public class CustomAuthenticationSatetProvider : AuthenticationStateProvider, ICustomAuthenticationService
    {
        private ISessionStorageService _storage;
        private IUserService _userService;
        private readonly IServiceProvider _serviceProvider;


        public CustomAuthenticationSatetProvider(ISessionStorageService storage, IUserService userService, IServiceProvider service)
        {
            this._storage = storage;
            this._userService = userService;
            this._serviceProvider = service;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _role = scope.ServiceProvider.GetRequiredService<RoleManager<RoleModel>>();
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
                            if (currentRole.Role != null && !string.IsNullOrEmpty(currentRole.Role.RoleID))
                            {
                                claims.Add(new System.Security.Claims.Claim(ClaimTypes.Role, currentRole.Role.RoleID));
                                var claimsFromRole = await _role.GetClaimsAsync(currentRole.Role);
                                foreach (var claim in claimsFromRole)
                                {
                                    claims.Add(claim);
                                }
                            }
                        }
                        identity = new ClaimsIdentity(claims, "apiauth_type");
                    }
                    var user = new ClaimsPrincipal(identity);
                    return await Task.FromResult(new AuthenticationState(user));
                }
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
                 if(currentRole.Role != null && !string.IsNullOrEmpty(currentRole.Role.RoleID))
                 {
                     claims.Add(new System.Security.Claims.Claim(ClaimTypes.Role, currentRole.Role.RoleID));
                 }
            }

            var identity = new ClaimsIdentity(claims, "apiauth_type");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task Logout()
        {
            await _storage.RemoveItemAsync("userID");
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
