using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Represents a role in the system
    public class RoleModel
    {
        public int? RoleNo { get; set; }
        [Required]
        public string RoleID { get; set; } // Required property for the unique identifier of the role
        [Required]
        public string RoleDesc { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<RoleModel>(Json);
        }
        public RoleModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
    }

    // Represents the association of a user with a branch and a role
    public class UserBranchRoleModel
    {
        public UserBranchRoleModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public bool IsSelected { get; set; }
        public bool IsDisabled { get; set; }
        public UserModel User { get; set; }
        public BranchModel Branch { get; set; }
        public RoleModel Role { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}