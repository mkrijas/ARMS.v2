using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class UserBranchRoleModel
    {
        public UserModel User { get; set; }
        public BranchModel Branch { get; set; }
        public RoleModel Role { get; set; }
    }
}
