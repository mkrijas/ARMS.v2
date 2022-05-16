using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class PartyBranchModel
    {
        public PartyBranchModel()
        {
            Address = new AddressModel(){ 
                AddresseeName = "Name",
                };
            Party = new PartyModel();
            UserInfo = new SharedModels.UserInfoModel();
            Contacts = new();
        }


        private string _tradeName;

        public int? GstID { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 15,ErrorMessage ="Gst number must have 15 characters")]
        public string GstNo { get; set; }

        [Required]
        [StringLength(200)]
        public string RegName { get; set; }

        [Required]
        [StringLength(maximumLength: 200)]
        public string TradeName { get { return _tradeName; } set { _tradeName = value; Address.AddresseeName = value; } }

        [Required]
        [StringLength(10, MinimumLength = 10,ErrorMessage ="Must have 10 characters")]
        public string TanNo { get; set; }

        public int? PartyID { get; set; }
        public int? AddressID { get; set; }      
        public virtual PartyModel Party { get; set; }        
        [ValidateComplexType]
        public AddressModel Address { get; set; }
        
        [ValidateComplexType]
        public List<ContactModel> Contacts { get; set; }

        public SharedModels.UserInfoModel UserInfo { get; set; }
    }



  


}
