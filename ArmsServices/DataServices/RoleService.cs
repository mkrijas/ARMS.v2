using ArmsModels.BaseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class RoleStore : IRoleStore<RoleModel>
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
               new SqlParameter("@RoleID", model.RoleID),
               new SqlParameter("@RoleName", model.RoleName),
               new SqlParameter("@UpdatedBy", model.UpdatedBy),
            };
            await Iservice.ExecuteNonQuery("[usp.user.RoleUpdate]", parameters);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(RoleModel model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleID", model.RoleID),
               new SqlParameter("@RoleName", model.RoleName),
               new SqlParameter("@UpdatedBy", model.UpdatedBy),
            };
            await Iservice.ExecuteNonQuery("[usp.user.RoleUpdate]", parameters);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(RoleModel model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleID", model.RoleID),
               new SqlParameter("@UpdatedBy", model.UpdatedBy),
            };
            await Iservice.ExecuteNonQuery("[usp.user.RoleDelete]", parameters);
            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(RoleModel model, CancellationToken cancellationToken)
        {
            return Task.FromResult(model.RoleID.ToString());
        }

        public Task<string> GetRoleNameAsync(RoleModel model, CancellationToken cancellationToken)
        {
            return Task.FromResult(model.RoleName);
        }

        public Task SetRoleNameAsync(RoleModel model, string roleName, CancellationToken cancellationToken)
        {
            model.RoleName = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(RoleModel role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.RoleName);
        }

        public Task SetNormalizedRoleNameAsync(RoleModel role, string normalizedName, CancellationToken cancellationToken)
        {
            role.RoleName = normalizedName;
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
            RoleModel role = new RoleModel();
            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Role.RoleSelect]", parameters))
            {
                role.RoleID = dr["RoleID"].ToString();
                role.RoleName = dr["RoleName"].ToString();
            }
            return role;
        }

        public async Task<RoleModel> FindByNameAsync(string RoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RoleName", RoleName),
               new SqlParameter("@operation", "FindByName"),
            };
            RoleModel role = new RoleModel();
            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Role.RoleSelect]", parameters))
            {
                role.RoleID =dr["RoleID"].ToString();
                role.RoleName = dr["RoleName"].ToString();
            }
            return role;
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}
