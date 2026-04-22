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
        private AuthenticationState _cachedState;


        public CustomAuthenticationSatetProvider(ISessionStorageService storage, IUserService userService, IServiceProvider service)
        {
            this._storage = storage;
            this._userService = userService;
            this._serviceProvider = service;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_cachedState != null)
            {
                return _cachedState;
            }

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _role = scope.ServiceProvider.GetRequiredService<RoleManager<RoleModel>>();
                    var userID = await _storage.GetItemAsync<string>("userID");
                    var identity = new ClaimsIdentity();

                    if (userID != null)
                    {
                        List<Claim> claims = new List<Claim> {  new Claim(ClaimTypes.Name, userID)  };

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
                                claims.AddRange(claimsFromRole);
                            }

                            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
                            var userAccount = await userManager.FindByIdAsync(userID);
                            if (userAccount != null)
                            {
                                var userClaims = await userManager.GetClaimsAsync(userAccount);
                                claims.AddRange(userClaims);
                            }
                        }
                        identity = new ClaimsIdentity(claims, "apiauth_type");
                    }
                    var user = new ClaimsPrincipal(identity);
                    _cachedState = new AuthenticationState(user);
                    return _cachedState;
                }
            }
            catch
            {
                _cachedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                return _cachedState;
            }
        
        }

        public async Task AuthenticateUser(string userID)
        {
            // 1. Start with basic identity claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userID),
                new Claim(ClaimTypes.NameIdentifier, userID) // Good practice for Identity
            };

            using (var scope = _serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleModel>>();
                var currentRole = _userService.GetCurrentBranchRole(userID);
                if (currentRole?.Branch != null)
                {
                    claims.Add(new Claim("BranchID", currentRole.Branch.BranchID.ToString()));

                    if (!string.IsNullOrEmpty(currentRole.Branch.BranchName))
                        claims.Add(new Claim("BranchName", currentRole.Branch.BranchName));

                    if (currentRole.Role != null)
                    {
                        // Add the role name claim
                        claims.Add(new Claim(ClaimTypes.Role, currentRole.Role.RoleID));

                        // 2. Fetch specific permission claims assigned to that role
                        var roleClaims = await roleManager.GetClaimsAsync(currentRole.Role);
                        claims.AddRange(roleClaims);
                    }

                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
                    var userAccount = await userManager.FindByIdAsync(userID);
                    if (userAccount != null)
                    {
                        var userClaims = await userManager.GetClaimsAsync(userAccount);
                        claims.AddRange(userClaims);
                    }
                }
            }

            // 3. Create the Principal
            var identity = new ClaimsIdentity(claims, "apiauth_type");
            var user = new ClaimsPrincipal(identity);

            // 4. Update the cached state and notify the UI
            _cachedState = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(_cachedState));

            // TIP: Save userID to LocalStorage here so GetAuthenticationStateAsync 
            // can rebuild this identity when the app restarts.
        }

        public async Task Logout()
        {
            await _storage.RemoveItemAsync("userID");
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            _cachedState = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(_cachedState));
        }
    }
}
