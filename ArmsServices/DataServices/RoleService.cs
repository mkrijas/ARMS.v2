using ArmsModels.BaseModels;
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
    public interface IRoleService<T>
    {
        Task<IList<T>> GetAllRoles(CancellationToken cancellationToken);
        Task<IList<Claim>> GetAllClaims(CancellationToken cancellationToken);
        IEnumerable<RoleModel> Select(string RoleID);
    }

    public class RoleStore : IRoleStore<RoleModel>,IRoleClaimStore<RoleModel>,IRoleService<RoleModel>
    {
        IDbService Iservice;
        public RoleStore(IDbService iservice)
        {
            Iservice = iservice;
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

        public Task AddClaimAsync(RoleModel role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleID", role.RoleID),
               new SqlParameter("ClaimType",claim.Type),
               new SqlParameter("ClaimValue",claim.Value),
               new SqlParameter("UserID",role.UserInfo.UserID)
            };

            Iservice.ExecuteNonQuery("[usp.RoleClaims.Update]", parameters);
            return Task.FromResult(0);
        }

        public Task RemoveClaimAsync(RoleModel role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleID", role.RoleID),
               new SqlParameter("ClaimType",claim.Type),
               new SqlParameter("ClaimValue",claim.Value)
            };

            Iservice.ExecuteNonQuery("[usp.RoleClaims.Delete]", parameters);
            return Task.FromResult(0);
        }
        private RoleModel GetModel(IDataRecord reader)
        {
            return new RoleModel
            {
                RoleID = reader.GetString("RoleID"),
                RoleNo = reader.GetInt32("RoleNo"),
                RoleDesc = reader.GetString("RoleDesc"),
              

            };
        }
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

        public Task<IList<Claim>> GetAllClaims(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();           

            IList<Claim> Claims = new List<Claim>();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.user.ClaimsMaster.Select]", null))
            {
                Claims.Add(new Claim(dr.GetString("ClaimType"), dr.GetString("ClaimValue")));
            };
            return Task.FromResult(Claims);
        }
    }
}
