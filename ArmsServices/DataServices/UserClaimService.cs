using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ArmsServices.DataServices
{  

    public class AddUserClaimsTransformation : IClaimsTransformation
    {
        private readonly IUserService _userService;

        public AddUserClaimsTransformation(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Clone current identity
            var clone = principal.Clone();
            var newIdentity = (ClaimsIdentity)clone.Identity;

            // Support AD and local accounts
            var nameId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier ||
                                                              c.Type == ClaimTypes.Name);
            if (nameId == null)
            {
                return principal;
            }

            // Get userInfo from database
            UserBranchRoleModel userInfo = await Task.Run(() => _userService.GetCurrentBranchRole(nameId.Value));
            if (userInfo?.User == null)
            {
                return principal;
            }            

            foreach(var item in newIdentity.Claims.ToList())
            {
                if(item.Type == ClaimTypes.Role)
                newIdentity.RemoveClaim(item);
            }            
            List<Claim> claims = new()
            {                
                new Claim("BranchID", userInfo.Branch.BranchID.ToString()),
                new Claim("BranchName", userInfo.Branch.BranchName),
                new Claim(newIdentity.RoleClaimType, userInfo.Role.RoleID)
            };
            newIdentity.AddClaims(claims);
            return clone;
        }
    }
}
