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
    public class UserStore : IUserStore<UserModel>, 
                             IUserEmailStore<UserModel>, 
                             IUserPhoneNumberStore<UserModel>,
                             IUserTwoFactorStore<UserModel>, 
                             IUserPasswordStore<UserModel>, 
                             IUserRoleStore<UserModel>, 
                             IUserClaimStore<UserModel>, 
                             IUserService
    {

        IDbService Iservice;
        public UserStore(IDbService iservice)
        {
            Iservice = iservice;
        }

        public UserStore()
        {
        }

        public async Task<IdentityResult> CreateAsync(UserModel model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", model.UserID),
               new SqlParameter("@UserName", model.UserName),
               new SqlParameter("@Email", model.Email),
               new SqlParameter("@EmailConfirmed", model.EmailConfirmed),
               new SqlParameter("@PasswordHash", model.PasswordHash),
               new SqlParameter("@PhoneNumber", model.PhoneNumber),
               new SqlParameter("@PhoneNumberConfirmed", model.PhoneNumberConfirmed),
               new SqlParameter("@UpdatedBy", model.UpdatedBy??model.UserID),
               new SqlParameter("@operation", "Insert"),
            };
            await Iservice.ExecuteNonQueryAsync("[usp.user.UserUpdate]", parameters);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(UserModel model, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", model.UserID),
               new SqlParameter("@UpdatedBy", model.UpdatedBy),
            };
            await Iservice.ExecuteNonQueryAsync("[usp.user.UserDelete]", parameters);

            return IdentityResult.Success;
        }

        public async Task<UserModel> FindByIdAsync(string userID, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", userID),
               new SqlParameter("@operation", "FindByID"),
            };

            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.user.UserSelect]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        public async Task<UserModel> FindByNameAsync(string UserID, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", UserID),
                new SqlParameter("@operation", "FindByID"),
            };
            UserModel user = new UserModel();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.user.UserSelect]", parameters))
            {
                user = GetModel(dr);
            }
            return user.UserID == null ? null : user;
        }

        public Task<string> GetNormalizedUserNameAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserID);
        }

        public Task<string> GetUserIdAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserID);
        }

        public Task<string> GetUserNameAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserID);
        }

        public Task SetNormalizedUserNameAsync(UserModel user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserID = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(UserModel user, string userName, CancellationToken cancellationToken)
        {
            user.UserID = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(UserModel model, CancellationToken cancellationToken)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", model.UserID),
               new SqlParameter("@UserName", model.UserName),
               new SqlParameter("@Email", model.Email),
               new SqlParameter("@EmailConfirmed", model.EmailConfirmed),
               new SqlParameter("@PasswordHash", model.PasswordHash),
               new SqlParameter("@PhoneNumber", model.PhoneNumber),
               new SqlParameter("@PhoneNumberConfirmed", model.PhoneNumberConfirmed),
               new SqlParameter("@UpdatedBy", model.UpdatedBy?? model.UserID),
               new SqlParameter("@operation", "Update"),
            };
            await Iservice.ExecuteNonQueryAsync("[usp.user.UserUpdate]", parameters);

            return IdentityResult.Success;
        }

        public Task SetEmailAsync(UserModel user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(UserModel user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task<UserModel> FindByEmailAsync(string Email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Email", Email),
               new SqlParameter("@operation", "FindByEmail"),
            };
            UserModel user = new UserModel();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.user.UserSelect]", parameters))
            {
                user = GetModel(dr);
            }
            return user?.UserID == null ? null : user;
        }

        public Task<string> GetNormalizedEmailAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task SetNormalizedEmailAsync(UserModel user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.Email = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(UserModel user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(UserModel user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(UserModel user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetPasswordHashAsync(UserModel user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(UserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }

        public Task AddToRoleAsync(UserModel user, string roleName, CancellationToken cancellationToken)
        {
            //
            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(UserModel user, string roleName, CancellationToken cancellationToken)
        {
            //
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(UserModel user, CancellationToken cancellationToken)
        {

            //to implement
            IList<string> strs = new List<string>();
            return Task.FromResult(strs);
        }

        public Task<bool> IsInRoleAsync(UserModel user, string roleName, CancellationToken cancellationToken)
        {
            //to implement
            return Task.FromResult(true);
        }

        public Task<IList<UserModel>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            //
            IList<UserModel> users = new List<UserModel>();
            return Task.FromResult(users);
        }

        public async Task<IList<Claim>> GetClaimsAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", user.UserID),
            };
            IList<Claim> claims = new List<Claim>();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.user.UserClaims.Select]", parameters))
            {
                claims.Add(new Claim(dr.GetString("ClaimType"), dr.GetString("ClaimValue")));
            }
            return claims;
        }

        public Task AddClaimsAsync(UserModel user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            DataTable dt = new();
            dt.Columns.Add("ClaimType");
            dt.Columns.Add("ClaimValue");
            foreach (Claim claim in claims)
            {
                DataRow row = dt.NewRow();
                row["ClaimType"] = claim.Type;
                row["ClaimValue"] = claim.Value;
                dt.Rows.Add(row);
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", user.UserID),
               new SqlParameter("@Claims", dt),
            };
            Iservice.GetDataReaderAsync("[usp.user.UserClaims.Update]", parameters);
            return Task.FromResult(0);
        }
        private UserModel GetModel(IDataRecord dr)
        {
            return new UserModel
            {
                UserID = dr["UserID"].ToString(),
                UserName = dr["UserName"].ToString(),
                PasswordHash = dr["PasswordHash"].ToString(),
                PhoneNumber = dr["PhoneNumber"].ToString(),
                PhoneNumberConfirmed = bool.Parse(dr["PhoneNumberConfirmed"].ToString()),
                Email = dr["Email"].ToString(),
                EmailConfirmed = bool.Parse(dr["EmailConfirmed"].ToString()),
                TwoFactorEnabled = bool.Parse(dr["TwoFactorEnabled"].ToString()),
                RecordStatus = byte.Parse(dr["RecordStatus"].ToString()),

            };
        }
        public Task ReplaceClaimAsync(UserModel user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            DataTable dt = new();
            dt.Columns.Add("ClaimType");
            dt.Columns.Add("ClaimValue");
            DataRow row = dt.NewRow();
            row["ClaimType"] = newClaim.Type;
            row["ClaimValue"] = newClaim.Value;
            dt.Rows.Add(row);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", user.UserID),
               new SqlParameter("@Claims", dt),
            };
            Iservice.GetDataReaderAsync("[usp.user.UserClaims.Update]", parameters);
            return Task.FromResult(0);
        }

        public Task RemoveClaimsAsync(UserModel user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserModel>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<UserModel> Select(string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", UserID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.user.UserSelect]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public IEnumerable<UserModel> SelectDeleted(string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@operation", "Deleted"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.user.UserSelect]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public int DeleteBranchesNRoles(UserBranchRoleModel model, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", model.User.UserID),
               new SqlParameter("@BranchID", model.Branch.BranchID),
                new SqlParameter("@DeletedBy", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.user.BranchRole.Delete]", parameters);
        }
        public int DeleteUser(string UserID, string DeletedBy)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", UserID),
                //new SqlParameter("@DeletedBy", DeletedBy),
            };
            return Iservice.ExecuteNonQuery("[usp.User.UserDelete]", parameters);
        }
        public IEnumerable<UserBranchRoleModel> GetBranchesNRoles(string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.user.BranchRole.Select]", parameters))
            {
                yield return new UserBranchRoleModel
                {
                    User = new UserModel() { UserID = dr.GetString("UserID") },
                    Branch = new BranchModel() { BranchID = dr.GetInt32("BranchID"), BranchName = dr.GetString("BranchName") },
                    Role = new RoleModel() { RoleID = dr.GetString("RoleID") },
                };
            }
        }
        public IEnumerable<UserBranchRoleModel> GetAllBranchesNRoles(string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.AllBranches.BranchRole.Select]", parameters))
            {
                yield return new UserBranchRoleModel
                {
                    User = new UserModel() { UserID = dr.GetString("UserID") },
                    Branch = new BranchModel() { BranchID = dr.GetInt32("BranchID"), BranchName = dr.GetString("BranchName") },
                    Role = new RoleModel() { RoleID = dr.GetString("RoleID"), RoleNo = dr.GetInt32("UserRoleID") },
                };
            }
        }

        public int SetBranchesNRoles(List<UserBranchRoleModel> lst, string UserID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("User");
            dt.Columns.Add("Branch");
            dt.Columns.Add("Role");
            foreach (UserBranchRoleModel item in lst)
            {
                DataRow row = dt.NewRow();
                row["User"] = item.User.UserID;
                row["Branch"] = item.Branch.BranchID;
                row["Role"] = item.Role.RoleID;
                dt.Rows.Add(row);
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@BranchRoles", dt )
            };
            return Iservice.ExecuteNonQuery("[usp.user.BranchRole.Update]", parameters);
        }

        public UserBranchRoleModel GetCurrentBranchRole(string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", UserID),
            };
            UserBranchRoleModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.user.BranchRole.Current.Select]", parameters))
            {
                model = new UserBranchRoleModel
                {
                    User = new UserModel() { UserID = dr.GetString("UserID") },
                    Branch = new BranchModel() { BranchID = dr.GetInt32("BranchID"), BranchName = dr.GetString("BranchName") },
                    Role = new RoleModel() { RoleID = dr.GetString("RoleID") },
                };
            }
            return model;
        }
        public int SetCurrentBranchRole(UserBranchRoleModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", model.User.UserID),
               new SqlParameter("@BranchID", model.Branch.BranchID ),
               new SqlParameter("@RoleID", model.Role.RoleID ),
            };
            return Iservice.ExecuteNonQuery("[usp.user.BranchRole.Current.Update]", parameters);
        }

        public int SelectDeviceExists(string UserID, string DeviceID, string Operation)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@DeviceID", DeviceID),
               new SqlParameter("@Operation", "APPROVE")
            };

            object result = Iservice.ExecuteScalar("[usp.User.DeviceDetails.Select]", parameters);

            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }
            else
            {
                return -1; // or a default value like 0
            }

        }
        public UserModel UpdateDeviceDetails(string UserID, string DeviceID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@DeviceID", DeviceID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DeviceDetails.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }
    }
}
