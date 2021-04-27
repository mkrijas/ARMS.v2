using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.SharedModels
{
    public class UserInfoModel
    {
        public byte? RecordStatus { get; set; }
        public string UserID { get; set; }
        public DateTime? TimeStampField { get; set; }
    }
}
