using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Views.Data
{
    public class CustomAuthenticationSatetProvider : AuthenticationStateProvider
    {
        private ISessionStorageService _storage;
        public CustomAuthenticationSatetProvider(ISessionStorageService storage)
        {
            this._storage = storage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userID = await _storage.GetItemAsync<string>("userID");
            var identity = new ClaimsIdentity();

            if (userID != null)
            {
                identity = new ClaimsIdentity(new[] 
                {
                  new Claim(ClaimTypes.Name,userID)
                }, "apiauth_type");
            } 
            var user = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(user));
        }


        public void AuthenticateUser(string userID)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name,userID)
            }, "apiauth_type");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
