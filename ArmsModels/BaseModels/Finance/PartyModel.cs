using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Reflection;

namespace ArmsModels.BaseModels
{
    // Model representing a party (e.g., supplier, customer, renter)
    public class PartyModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<PartyModel>(Json);
        }
        private string _tradeName;
        public PartyModel()
        {

        }
        public int? PartyID { get; set; } = 0; // Unique identifier for the party
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
        public string NatureOfBusiness { get; set; } //Supplier/Customer/Renter//SisterConcern
        public virtual string AssesseeType { get; set; }
        [Required]
        public bool PanAvailable { get; set; }
        [RequiredIfTrue("PanAvailable")]
        //[StringLength(10, MinimumLength = 10, ErrorMessage = "PAN must be 10 digits!")]
        public string PAN { get; set; }
        [Required]
        public bool TdsApplicable { get; set; }
        [Required]
        public string GstType { get; set; }// Registered,UnRegistered,Export,Deemed Export,Exempted,SEZ
        [RequiredIf("GstType", "Registered")]
        public string GstRegType { get; set; }  // GSTIN,UID,GID
        [RequiredIf("GstType", "Registered")]
        //[StringLength(15, ErrorMessage = "Gst number must have 15 characters")]
        public string GstNo { get; set; }
        [Required]
        public int? GstStateCode { get; set; }

        //[StringLength(10, MinimumLength = 10, ErrorMessage = "Must be 10 characters")]
        public string TanNo { get; set; }
        public int? CreditPeriod { get; set; } // Days
        public decimal? CreditLimit { get; set; }
        public string GoodsAndServiceType { get; set; } // Bank/Cash
        public string PaymentMode { get; set; } // Bank/Cash
        [RequiredIf("NatureOfBusiness", "Supplier")]
        public VendorPostingGroupModel VendorPostingGroup { get; set; } // Vendor posting group
        [RequiredIf("NatureOfBusiness", "Customer")]
        public CustomerPostingGroupModel CustomerPostingGroup { get; set; } // Customer posting group
        [RequiredIf("NatureOfBusiness", "Renter")]
        public RenterPostingGroupModel RenterPostingGroup { get; set; } // Renter posting grou 
        [RequiredIf("NatureOfBusiness", "SisterConcern")]
        public SisterPostingGroupModel SisterPostingGroup { get; set; } // Sister concern posting group
        [Required]
        public bool InterCompany { get; set; }
        public string IcPartnerCode { get; set; }

        [ValidateComplexType]
        public AddressModel Address { get; set; } = new(); // Address associated with the party
        [ValidateComplexType]
        [RequiredIf("PaymentMode", "Bank")]
        public BankAccountModel BankAccount { get; set; } // Bank account associated with the party

        [ValidateComplexType]
        public List<ContactModel> Contacts { get; set; } = new(); // List of contacts associated with the party
        public SharedModels.UserInfoModel UserInfo { get; set; } = new(); // Information about the user associated with the party
        public virtual string PostingGroupTitle { get; set; }
    }

    // Model representing a vendor posting group
    public class VendorPostingGroupModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<VendorPostingGroupModel>(Json);
        }
        public int? VendorPostingGroupID { get; set; } // Unique identifier for the vendor posting group
        [Required]
        public string Title { get; set; }        
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel Payable { get; set; } // Payable account for the vendor
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel PrePayment { get; set; } // Prepayment account for the vendor
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel Deposit { get; set; } // Deposit account for the vendor
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }

    // Model representing a sister posting group
    public class SisterPostingGroupModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<SisterPostingGroupModel>(Json);
        }
        public int? SisterPostingGroupID { get; set; } // Unique identifier for the sister posting group
        [Required]
        public string Title { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel Trade { get; set; } // Trade account for the sister posting group
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel PrePayment { get; set; } // Prepayment account for the sister posting group
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel Deposit { get; set; } // Deposit account for the sister posting group
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }

    // Model representing a customer posting group
    public class CustomerPostingGroupModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CustomerPostingGroupModel>(Json);
        }
        public int? CustomerPostingGroupID { get; set; } // Unique identifier for the customer posting group
        [Required]
        public string Title { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel Receivable { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel PrePayment { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel Deposit { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }

    // Model representing a renter posting group
    public class RenterPostingGroupModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<RenterPostingGroupModel>(Json);
        }
        public int? RenterPostingGroupID { get; set; }
        [Required]
        public string Title { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel Rent { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel Deposit { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel Other { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}