using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ArmsModels.BaseModels
{

    public class DataAuthorizationModel
    {
        public int? ID { get; set; }
        public int? DocumentID { get; set; }
        public int? DocTypeID { get; set; }
        public int? AuthLevelID { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class DocTypeModel
    {
        public int? ID { get; set; }
        public string Description { get; set; }
    }

    public class DataAuthorizationSettingsModel
    {
        public DataAuthorizationSettingsModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? ID { get; set; }
        [Required]
        public int? AuthLevelID { get; set; }
        [Required]
        public int? DocTypeID { get; set; }
        [Required]
        public virtual string DocType { get; set; }
        public virtual string AuthorizeType { get; set; }
        public string RoleID { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class DataAuthorizationTypeModel
    {
        public int? AuthLevelID { get; set; }
        public string Description { get; set; }        
        public bool IsApproval { get; set; }
    }
}
