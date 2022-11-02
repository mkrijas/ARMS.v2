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
        public int? PartyID { get; set; }
        [Required]
        [StringLength(maximumLength: 8)]
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
        public VendorPostingGroup VendorPostingGroup { get; set; } = new();
        public CustomerPostingGroup CustomerPostingGroup { get; set; } = new();
        public RenterPostingGroup RenterPostingGroup { get; set; } = new();
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

    public class VendorPostingGroup
    {
        public int? VendorPostingGroupID { get; set; }
        public string Title { get; set; }
        public int? Payable { get; set; }
        public int? PrePayment { get; set; }
        public int? Deposit { get; set; } 
    }

    public class CustomerPostingGroup
    {
        public int? CustomerPostingGroupID { get; set; }
        public string Title { get; set; }
        public int? Receivable { get; set; }
        public int? PrePayment { get; set; }
        public int? Deposit { get; set; }
    }

    public class RenterPostingGroup
    {
        public int? RenterPostingGroupID { get; set; }
        public string Title { get; set; }
        public int? Rent { get; set; }
        public int? Deposit { get; set; }
        public int? Other { get; set; }
    }
}
