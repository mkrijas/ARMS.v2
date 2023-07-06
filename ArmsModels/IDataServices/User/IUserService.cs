using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace ArmsServices.DataServices
{
    public interface IUserService
    {
        IEnumerable<UserBranchRoleModel> GetBranchesNRoles(string UserID);
        IEnumerable<UserBranchRoleModel> GetAllBranchesNRoles(string UserID);
        int SetBranchesNRoles(List<UserBranchRoleModel> lst, string UserID);
        int DeleteBranchesNRoles(UserBranchRoleModel model, string UserID);
        int DeleteUser(string UserID, string DeletedBy);
        UserBranchRoleModel GetCurrentBranchRole(string UserID);
        int SetCurrentBranchRole(UserBranchRoleModel model);
        IEnumerable<UserModel> Select(string UserID);
        IEnumerable<UserModel> SelectDeleted(string UserID);
    }
}