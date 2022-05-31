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
            Contacts = new();
        }

        public PartyModel(string _asseseeType)
        {
            UserInfo = new SharedModels.UserInfoModel();
            Contacts = new();
            AssesseeType = _asseseeType;
        }

        public int? PartyID { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string PartyName { get; set; }
        public bool IsClient { get; set; }
        public bool IsSupplier { get; set; }
        [Required]
        public int? AssesseeTypeID { get; set; }
        public string AssesseeType { get; }
        [Required]
        [StringLength(10,MinimumLength =10,ErrorMessage = "PAN must be 10 digits!")]
        public string PAN { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string NatureOfFirm { get; set; }//Proprietor, Partnership or Company
        [Required]
        public bool TdsApplicable { get; set; }
        [Required]
        public bool TcsApplicable { get; set; }
        [ValidateComplexType]
        public List<ContactModel> Contacts { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }

    }
}
