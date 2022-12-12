using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{

    public class RoleModel
    {
        public int? RoleNo { get; set; }
        [Required]
        public string RoleID { get; set; }
        [Required]
        public string RoleDesc { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }

        public RoleModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
    }

    public class UserBranchRoleModel
    {
        public UserModel User { get; set; }
        public BranchModel Branch { get; set; }
        public RoleModel Role { get; set; }
        //public UserInfoModel UserInfo_ { get; set; }
    }

  
}
