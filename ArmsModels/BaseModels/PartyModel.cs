using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class PartyModel
    {
        public PartyModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }

        public int PartyID { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string PartyName { get; set; }
        public bool IsClient { get; set; }
        public bool IsSupplier { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string NatureOfFirm { get; set; }//Proprietor, Partnership or Company
        [Required]
        public bool TdsApplicable { get; set; }
        [Required]
        public bool TcsApplicable { get; set; }        
public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
