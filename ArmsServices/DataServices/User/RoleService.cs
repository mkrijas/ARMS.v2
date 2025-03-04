using ArmsModels.BaseModels;
using ArmsModels.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class RoleStore : IRoleStore<RoleModel>, IRoleClaimStore<RoleModel>, IRoleService<RoleModel>
    {
        IDbService Iservice;
        AuthenticationStateProvider auth;
        public RoleStore(IDbService iservice, AuthenticationStateProvider _auth)
        {
            Iservice = iservice;
            this.auth = _auth;
        }

        public async Task<IdentityResult> CreateAsync(RoleModel model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleNo", model.RoleNo),
               new SqlParameter("@RoleID", model.RoleID),
               new SqlParameter("@RoleDesc", model.RoleDesc),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            await Iservice.ExecuteNonQueryAsync("[usp.user.Roles.Update]", parameters);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(RoleModel model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleNo", model.RoleNo),
               new SqlParameter("@RoleID", model.RoleID),
               new SqlParameter("@RoleDesc", model.RoleDesc),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            await Iservice.ExecuteNonQueryAsync("[usp.user.Roles.Update]", parameters);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(RoleModel model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleID", model.RoleID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            await Iservice.ExecuteNonQueryAsync("[usp.user.Roles.Delete]", parameters);
            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(RoleModel model, CancellationToken cancellationToken)
        {
            return Task.FromResult(model.RoleID.ToString());
        }

        public Task<string> GetRoleNameAsync(RoleModel model, CancellationToken cancellationToken)
        {
            return Task.FromResult(model.RoleDesc);
        }

        public Task SetRoleNameAsync(RoleModel model, string roleName, CancellationToken cancellationToken)
        {
            model.RoleDesc = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(RoleModel role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.RoleDesc);
        }

        public Task SetNormalizedRoleNameAsync(RoleModel role, string normalizedName, CancellationToken cancellationToken)
        {
            role.RoleDesc = normalizedName;
            return Task.FromResult(0);
        }

        public async Task<RoleModel> FindByIdAsync(string roleID, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@roleID", roleID),
               new SqlParameter("@operation", "FindById"),
            };
            RoleModel role = null;
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[User.Roles.Select]", parameters))
            {
                role = new RoleModel()
                {
                    RoleNo = dr.GetInt32("RoleNo"),
                    RoleID = dr.GetString("RoleID"),
                    RoleDesc = dr.GetString("RoleDesc")
                };
            }
            return role;
        }

        public async Task<RoleModel> FindByNameAsync(string RoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleDesc", RoleName),
               new SqlParameter("@operation", "FindByName"),
            };
            RoleModel role = null;
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.User.Roles.Select]", parameters))
            {
                role = new RoleModel()
                {
                    RoleNo = dr.GetInt32("RoleNo"),
                    RoleID = dr.GetString("RoleID"),
                    RoleDesc = dr.GetString("RoleDesc")
                };
            }
            return role;
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }

        public Task<IList<Claim>> GetClaimsAsync(RoleModel role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleID", role.RoleID)
            };

            IList<Claim> Claims = new List<Claim>();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.RoleClaims.Select]", parameters))
            {
                Claims.Add(new Claim(dr.GetString("ClaimType"), dr.GetString("ClaimValue")));
            };
            return Task.FromResult(Claims);
        }

        // Method to check if a role has a specific claim
        public async Task<bool> HasClaim(string DocTypeID, string ClaimValue, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var authprov = await auth.GetAuthenticationStateAsync();
           // string RoleID = "";
            //if (authprov.User.Claims.Any(x => x.Type == ClaimTypes.Role))
            //    RoleID = authprov.User.Claims.First(x => x.Type == ClaimTypes.Role).Value;

            //List<SqlParameter> parameters = new List<SqlParameter>
            //{
            //   new SqlParameter("@RoleID", RoleID)
            //};

            //IList<Claim> Claims = new List<Claim>();

            //foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.RoleClaims.Select]", parameters))
            //{
            //    Claims.Add(new Claim(dr.GetString("ClaimType"), dr.GetString("ClaimValue")));
            //};
            return authprov.User.Claims.Any(s => s.Type == DocTypeID && s.Value == ClaimValue);
        }

        public Task AddClaimAsync(RoleModel role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleID", role.RoleID),
               new SqlParameter("ClaimType",claim.Type),
               new SqlParameter("ClaimValue",claim.Value),
               new SqlParameter("UserID",role.UserInfo.UserID),
               new SqlParameter("Operation","CHECK"),
            };

            Iservice.ExecuteNonQuery("[usp.user.RoleClaims.Update]", parameters);
            return Task.FromResult(0);
        }

        public Task RemoveClaimAsync(RoleModel role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleID", role.RoleID),
               new SqlParameter("ClaimType",claim.Type),
               new SqlParameter("ClaimValue",claim.Value),
               new SqlParameter("Operation","UNCHECK"),
            };

            Iservice.ExecuteNonQuery("[usp.User.RoleClaims.Update]", parameters);
            return Task.FromResult(0);
        }

        // Method to select all permissions for a role
        public void SelectAllPermissions(RoleModel role, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleID", role.RoleID),
               new SqlParameter("UserID",UserID),
               new SqlParameter("Operation","CHECKALL"),
            };
            Iservice.ExecuteNonQuery("[usp.user.RoleClaims.Update]", parameters);
        }

        // Method to deselect all permissions for a role
        public void DeSelectAllPermissions(RoleModel role, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleID", role.RoleID),
               new SqlParameter("UserID",UserID),
               new SqlParameter("Operation","UNCHECKALL"),
            };

            Iservice.ExecuteNonQuery("[usp.user.RoleClaims.Update]", parameters);
        }

        // Private method to convert an IDataRecord to a RoleModel
        private RoleModel GetModel(IDataRecord reader)
        {
            return new RoleModel
            {
                RoleID = reader.GetString("RoleID"),
                RoleNo = reader.GetInt32("RoleNo"),
                RoleDesc = reader.GetString("RoleDesc"),
            };
        }

        // Method to select roles by RoleID
        public IEnumerable<RoleModel> Select(string RoleID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleID",RoleID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.Roles.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to get all roles
        public Task<IList<RoleModel>> GetAllRoles(CancellationToken cancellationToken = default)
        {
            IList<RoleModel> Roles = new List<RoleModel>();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.Roles.Select]", null))
            {
                Roles.Add(new RoleModel()
                {
                    RoleNo = dr.GetInt32("RoleNo"),
                    RoleID = dr.GetString("RoleID"),
                    RoleDesc = dr.GetString("RoleDesc"),
                });
            };
            return Task.FromResult(Roles);
        }
        // Method to get all claims
        public Task<IList<Claim>> GetAllClaims(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IList<Claim> Claims = new List<Claim>();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.user.ClaimsMaster.Select]", null))
            {
                Claims.Add(new Claim(dr.GetString("ClaimType"), dr.GetString("ClaimValue"), dr.GetString("ClaimGroup"), dr.GetString("ParentGroup")));
            };
            return Task.FromResult(Claims);
        }
    }
}