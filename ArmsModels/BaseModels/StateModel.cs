using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class StateModel
    {        
            public int? StateID { get; set; }
            [Required]
            [StringLength(maximumLength: 200)]
            public string StateName { get; set; }   
            public SharedModels.UserInfoModel UserInfo { get; set; }       
    }
}
