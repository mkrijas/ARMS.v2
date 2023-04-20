using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class PartyModel
    {
        private string _tradeName;
        public PartyModel()
        {                   
        }
        public int? PartyID { get; set; } = 0;
        public string PartyCode { get; set; }        

        [Required]
        [StringLength(maximumLength: 200)]
        public string TradeName
        {
            get
            { return _tradeName; }
            set { _tradeName = value; Address.AddresseeName = value; }
        }
        [Required]
        [StringLength(200)]
        public string RegName { get; set; }
        public string NatureOfBusiness { get; set; } //Supplier/Customer/Renter 
        public virtual string AssesseeType { get; set; }
        [Required]
        public bool PanAvailable { get; set; }
        [RequiredIfTrue("PanAvailable")]
        [StringLength(10,MinimumLength =10,ErrorMessage = "PAN must be 10 digits!")]
        public string PAN { get; set; }        
        [Required]
        public bool TdsApplicable { get; set; } 
        public string GstType { get; set; }// Registered,UnRegistered,Export,Deemed Export,Exempted,SEZ
        public string GstRegType { get; set; }  // GSTIN,UID,GID
        
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Gst number must have 15 characters")]
        public string GstNo { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Must have 10 characters")]
        public string TanNo { get; set; }
        public int? CreditPeriod { get; set; } // Days
        public decimal? CreditLimit { get; set; }
        public string PaymentMode { get; set; } // Bank/Cash
        [RequiredIf("NatureOfBusiness", "Supplier")]
        public VendorPostingGroupModel VendorPostingGroup { get; set; }
        [RequiredIf("NatureOfBusiness", "Customer")]
        public CustomerPostingGroupModel CustomerPostingGroup { get; set; }
        [RequiredIf("NatureOfBusiness", "Renter")]
        public RenterPostingGroupModel RenterPostingGroup { get; set; }
        [Required]
        public bool InterCompany { get; set; }
        public string IcPartnerCode { get; set; }

        [ValidateComplexType]
        public AddressModel Address { get; set; } = new();
        [ValidateComplexType]
        public BankAccountModel BankAccount { get; set; } = new();

        [ValidateComplexType]
        public List<ContactModel> Contacts { get; set; } = new();
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    } 

    public class VendorPostingGroupModel
    {
        public int? VendorPostingGroupID { get; set; }
        public string Title { get; set; }
        public ChartOfAccountModel Payable { get; set; } = new();
        public ChartOfAccountModel PrePayment { get; set; } = new();
        public ChartOfAccountModel Deposit { get; set; } = new();
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }

    public class CustomerPostingGroupModel
    {
        public int? CustomerPostingGroupID { get; set; }
        public string Title { get; set; }
        public ChartOfAccountModel Receivable { get; set; } = new();
        public ChartOfAccountModel PrePayment { get; set; } = new();
        public ChartOfAccountModel Deposit { get; set; } = new();
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }

    public class RenterPostingGroupModel
    {
        public int? RenterPostingGroupID { get; set; }
        public string Title { get; set; }
        public ChartOfAccountModel Rent { get; set; } = new();
        public ChartOfAccountModel Deposit { get; set; } = new();
        public ChartOfAccountModel Other { get; set; } = new();
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}
