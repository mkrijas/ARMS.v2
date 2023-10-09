using Newtonsoft.Json;
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