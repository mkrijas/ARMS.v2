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
        private RoleManager<RoleModel> _role;
        

        public AddUserClaimsTransformation(IUserService userService,RoleManager<RoleModel> role)
        {
            _userService = userService;
            _role = role;
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

            foreach (var item in newIdentity.Claims.ToList())
            {
                if (item.Type == ClaimTypes.Role)
                    newIdentity.RemoveClaim(item);
            }

            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim("BranchID", userInfo.Branch.BranchID.ToString()));
            claims.Add(new Claim("BranchName", userInfo.Branch.BranchName));
            claims.Add(new Claim(newIdentity.RoleClaimType, userInfo.Role.RoleID));
            
            newIdentity.AddClaims(claims);
            return clone;
        }
    }
}
