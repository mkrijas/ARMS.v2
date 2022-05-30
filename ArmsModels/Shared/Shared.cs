using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ArmsModels.SharedModels
{
    public class UserInfoModel
    {
        public string UserID { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public byte? RecordStatus { get; set; }        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? TimeStampField { get; set; }
    }

    public class ApprovedInfoModel
    {
        public string ApprovedBy { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool Approved { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? ApprovedOn { get; set; }
    }

}
