using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class DistrictModel
    {
        public int DistrictID { get; set; }
        [Required][StringLength(maximumLength:200)]
        public string DistrictName { get; set; }
        [Required]
        public int StateID { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
