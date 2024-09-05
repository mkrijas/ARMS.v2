using ArmsModels.BaseModels;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MobileAPI.Services
{
    public interface IRoleService
    {
        Task<IList<Claim>> GetAllClaims(CancellationToken cancellationToken);
        Task<IList<Claim>> GetClaimsAsync(RoleModel role, CancellationToken cancellationToken = default);
        IEnumerable<RoleModel> Select(string RoleID);
        Task<bool> HasClaim(string DocTypeID, string ClaimValue, CancellationToken cancellationToken = default);
        void SelectAllPermissions(RoleModel role, string UserID);
        void DeSelectAllPermissions(RoleModel role, string UserID);

    }
}
