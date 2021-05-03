using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TruckRegistrationModel
    {
        public int RegID { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string RegNo { get; set; }
        public int TruckID { get; set; }
        public DateTime EffectFrom { get; set; }
        public DateTime EffectTo { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string RC { get; set; }  //Url of RC doc      
        public SharedModels.UserInfoModel UserInfo { get; set; }

        
    }
}
