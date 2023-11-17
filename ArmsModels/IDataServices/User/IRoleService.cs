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
        Task<IList<Claim>> GetClaimsAsync(RoleModel role, CancellationToken cancellationToken = default);
        IEnumerable<RoleModel> Select(string RoleID);
        Task<bool> HasClaim(string DocTypeID, string ClaimValue, CancellationToken cancellationToken = default);
        Task<bool> SelectAllPermissions(CancellationToken cancellationToken = default);
        Task<bool> DeSelectAllPermissions(CancellationToken cancellationToken = default);
    }
}