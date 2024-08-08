using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ArmsModels.BaseModels
{
    public class DataAuthorizationModel
    {
        public long? ID { get; set; }
        [Required]
        public int? DocumentID { get; set; }
        [Required]
        public int? DocTypeID { get; set; }
        [Required]
        public int? AuthLevelID { get; set; }
        public string Remarks { get; set; }
        public string ClaimValue { get; set; }
        public virtual string AuthType { get; set; }
        public virtual bool IsCompleted { get; set; } = false;
        public virtual string DisplayString { get { return string.Concat(UserInfo?.UserID, " at ", UserInfo?.TimeStampField?.ToString("dd/MM/yy HH:mm")); } }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }

    public class DocTypeModel
    {
        public int? ID { get; set; }
        public string Description { get; set; }
        public bool AuthImplemented { get; set; }
    }

    public class DataAuthorizationSettingsModel : ICloneable
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
        public virtual string DocType { get; set; }
        public virtual string AuthorizeType { get; set; }
        public string RoleID { get; set; }
        public string ClaimValue { get; set; }

        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DataAuthorizationSettingsModel>(Json);
        }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class DataAuthorizationTypeModel
    {
        public int? AuthLevelID { get; set; }
        public string Description { get; set; }
        public bool IsApproval { get; set; }
    }
    public class DataApprovedStatus
    {
        public int AuthLevelID { get; set; }
        public bool IsApprove { get; set; }
        public string Remarks { get; set; }

    }
}
