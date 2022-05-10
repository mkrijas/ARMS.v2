using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class GstModel
    {
        public GstModel()
        {
            Address = new AddressModel(){ 
                AddresseeName = "Name",
                };
            Party = new PartyModel();
            UserInfo = new SharedModels.UserInfoModel();
            Contacts = new();
        }
        public int? GstID { get; set; }
        [Required]
        [StringLength(maximumLength: 15, MinimumLength = 15)]
        public string GstNo { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string RegName { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string TradeName { get; set; }
        [Required]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string TanNo { get; set; }
        public int? PartyID { get; set; }
        public int? AddressID { get; set; }      
        public virtual PartyModel Party { get; set; }        
        [ValidateComplexType]
        public AddressModel Address { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        [ValidateComplexType]
        public List<ContactModel> Contacts { get; set; }
    }



    public class GstUsageIDModel
    {
        public GstUsageIDModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }

        [Required]
        [StringLength(maximumLength: 25)]
        public string UsageID { get; set; }
        [Required]
        public int? AccountID { get; set; }
        [Required]
        public int? RID { get; set; }

        [StringLength(maximumLength: 6)]
        public string SAC { get; set; }
        [Required]
        public DateTime? PeriodFrom { get; set; }
        [Required]
        public DateTime? PeriodTo { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        
    }


    public class GstRateModel
    {
        public int? RID { get; set; }
        public decimal? TaxRate { get; set; }
        public string Description { get; set; }
    }


}
