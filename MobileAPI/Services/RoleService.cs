using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MobileAPI.Services
{
    public class RoleService : IRoleStore<RoleModel>, IRoleClaimStore<RoleModel>, IRoleService<RoleModel>
    {
        IDbService Iservice;
        //AuthenticationStateProvider auth;
        public RoleService(IDbService iservice)//, AuthenticationStateProvider _auth
        {
            Iservice = iservice;
            //this.auth = _auth;
        }
        public Task AddClaimAsync(RoleModel role, Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(RoleModel role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(RoleModel role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void DeSelectAllPermissions(RoleModel role, string UserID)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public Task<RoleModel?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<RoleModel?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetAllClaims(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<RoleModel>> GetAllRoles(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(RoleModel role, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            IList<Claim> claims = new List<Claim>();
            return Task.FromResult(claims);
        }

        public Task<string?> GetNormalizedRoleNameAsync(RoleModel role, CancellationToken cancellationToken)
        {
            return Task.FromResult("");
        }

        public Task<string> GetRoleIdAsync(RoleModel role, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            return Task.FromResult("");
        }

        public Task<string?> GetRoleNameAsync(RoleModel role, CancellationToken cancellationToken)
        {
            return Task.FromResult("");
        }

        public async Task<bool> HasClaim(string DocTypeID, string ClaimValue, CancellationToken cancellationToken = default)
        {
            //cancellationToken.ThrowIfCancellationRequested();
            //var authprov = await auth.GetAuthenticationStateAsync();
            //return authprov.User.Claims.Any(s => s.Type == DocTypeID && s.Value == ClaimValue);
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(RoleModel role, Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RoleModel> Select(string RoleID)
        {
            throw new NotImplementedException();
        }

        public void SelectAllPermissions(RoleModel role, string UserID)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(RoleModel role, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(RoleModel role, string? roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(RoleModel role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
