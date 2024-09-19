using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ArmsModels.SharedModels;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class AssetModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<AssetModel>(Json);
        }
        public long? ID { get; set; }
        public int? PID { get; set; }
        public int? AssetID { get; set; }
        [Required]
        public string Description { get; set; }
        public int? ParentAssetID { get; set; }
        [Required]
        public bool IsComplex { get; set; } = false;
        [StringLength(8)]
        public virtual string AssetCode { get; set; }
        public virtual string Images { get; set; } = "";
        public virtual List<string> ImagePath { get; set; }
        public string SerialNumber { get; set; }
        [Required]
        public string NatureOfAsset { get; set; }//Tangible, Intangible
        [Required]
        public AssetClassModel AssetClass { get; set; } // Building,Vehicle,Computer etc
        [Required]
        public AssetSubClassModel SubClass { get; set; } // Printers,Chair,Engine etc
        [Required]
        public int? BranchID { get; set; }
        [RequiredIf("IsComplex", " false")]
        public int? GstRateID { get; set; }
        [RequiredIf("IsComplex", " false")]
        public string GstMechanism { get; set; } // FCM/RCM/INELIGIBLE
        [RequiredIf("IsComplex", " false")]
        [StringLength(10)]
        public string HsnCode { get; set; }
        [RequiredIf("IsComplex", " false")]
        public DateTime? WarrentyDate { get; set; }
        [RequiredIf("IsComplex", " false")]
        public PartyModel VendorInfo { get; set; }
        [RequiredIf("IsComplex", " false")]
        public string DepreciationBookCode { get; set; }// Income Tax,Company Act
        [RequiredIf("IsComplex", " false")]
        public string DepreciationMethod { get; set; }// Straigt Line,Diminishing Balance,Sum of Years Digits 
        [RequiredIf("IsComplex", " false")]
        public decimal? RateOfDepreciation { get; set; }
        [RequiredIf("IsComplex", " false")]
        public decimal? SalvageValue { get; set; }
        [RequiredIf("IsComplex", " false")]
        public decimal? BookValue { get; set; }
        [RequiredIf("IsComplex", " false")]
        public DateTime? ProjectedDisposalDate { get; set; }
        [RequiredIf("IsComplex", " false")]
        public int? GetAccountRuleDefinition { get; set; }
        [Required]
        public string AssetStatus { get; set; }
        //[RequiredIf("IsComplex", " false")]
        [RequiredIfComplexAndAssetStatus("IsComplex", " false", "AssetStatus", "Ready to use")]
        public DateTime? DepreciationStartingDate { get; set; }
        //[RequiredIf("IsComplex", " false")]
        [RequiredIfComplexAndAssetStatus("IsComplex", " false", "AssetStatus", "Ready to use")]
        public DateTime? DepreciationEndingDate { get; set; }

        public decimal? CurrentValue { get; set; }
        public decimal? TotalValue { get; set; }
        //[ExpressiveAnnotations.Attributes.RequiredIf("IsComplex == false")]
        public decimal? SpanOfYear { get; set; }
        public bool IsSold { get; set; }
        public virtual decimal? DepreciableValue
        {
            get
            {
                return BookValue - SalvageValue;
            }
        }
        public bool Scrap { get; set; } = false;
        public string Status { get; set; }//Scrap,Dismantled,Sold,Revaluated        
        public UserInfoModel UserInfo { get; set; } = new();
        public decimal? GSTValue { get; set; }
        public string AccountName { get; set; }
        public int? CoaID { get; set; }
        public decimal? TaxRate { get; set; }
    }

    public class AssetViewModel
    {
        public AssetModel Parent { get; set; }
        public string Description { get; set; }
        public List<AssetViewModel> Children { get; set; } = new();
    }

    public class AssetClassModel
    {
        public int? AssetClassID { get; set; }
        public string AssetClassName { get; set; }
        public int? PostingGroupID { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class AssetSubClassModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<AssetSubClassModel>(Json);
        }
        public int? ID { get; set; }
        public int? AssetSubClassID { get; set; }
        [Required(ErrorMessage = "Name of Asset SubClass is required !")]
        public virtual string AssetSubclass { get; set; }
        [Required(ErrorMessage = "Asset SubClass Abbreviation is required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Asset SubClass Abbreviation must have 3 characters !")]
        public string AssetSubAbbrev { get; set; }
        [Required(ErrorMessage = "AssetClass is required !")]
        public int? AssetClassID { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class AssetStatusUpdateModel
    {
        public int? StatusUpdateID { get; set; }
        [Required]
        public int? AssetID { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime? StatusDate { get; set; }
        public int? AccountTransactionID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public decimal? CumulativeAmount { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class AssetPostingGroupModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<AssetPostingGroupModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public string Title { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel Capitalization { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel CWIP { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel AccummulatedDepreciation { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel Depreciation { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel Revaluation { get; set; }
        [ValidateComplexType]
        [Required]
        public ChartOfAccountModel RevaluationReserve { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class AssetDepreciationModel
    {
        public int? ID { get; set; }
        public int? AssetID { get; set; }
        public DateTime? DateOfDepreciation { get; set; }
        public decimal? ChangeInValue { get; set; }
        public string DepreciationMethod { get; set; }// Straigt Line,Diminishing Balance,Sum of Years Digits         
        public decimal? RateOfDepreciation { get; set; }
        public int? AccountTransactionID { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class AssetPurchaseModel : ICloneable
    {
        public int? AssetID { get; set; }
        [Required]
        public DateTime? InitiatedDocumentDate { get; set; } = DateTime.Today;
        public string DocumentNumber { get; set; }
        public int? BranchID { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<AssetPOModel> SelectedAssets { get; set; } = new();
        public int? AuthLevelId { get; set; }
        public string AuthStatus { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<AssetPurchaseModel>(Json);
        }
    }

    public class AccountRuleDefModel
    {
        public int? ID { get; set; }
        public string? Title { get; set; }
        public int? CapitalizationID { get; set; }
        public int? CWIPID { get; set; }
    }

    public class AssetPOModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<AssetPOModel>(Json);
        }
        public long? ID { get; set; }
        public int? PID { get; set; }
        public int? AssetID { get; set; }
        [Required]
        public string Description { get; set; }
        public string AccountName { get; set; }
        [StringLength(8)]
        public string AssetCode { get; set; }
        [RequiredIf("IsComplex", " false")]
        public decimal? BookValue { get; set; }
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public int? CoaID { get; set; }
        public decimal? CurrentValue { get; set; }
        public virtual decimal? DepreciableValue
        {
            get
            {
                return BookValue - SalvageValue;
            }
        }
        [RequiredIf("IsComplex", " false")]
        public string DepreciationBookCode { get; set; }// Income Tax,Company Act
        [RequiredIf("IsComplex", " false")]
        public DateTime? DepreciationEndingDate { get; set; }
        [RequiredIf("IsComplex", " false")]
        public string DepreciationMethod { get; set; }// Straigt Line,Diminishing Balance,Sum of Years Digits 
        [RequiredIf("IsComplex", " false")]
        public DateTime? DepreciationStartingDate { get; set; }
        public decimal? GSTValue { get; set; }
        public decimal? CGSTValue { get; set; }
        public decimal? SGSTValue { get; set; }
        public decimal? IGSTValue { get; set; }
        public decimal? TDS { get; set; }
        [RequiredIf("IsComplex", " false")]
        public int? GetAccountRuleDefinition { get; set; }
        [RequiredIf("IsComplex", " false")]
        public string GstMechanism { get; set; } // FCM/RCM/INELIGIBLE
        [RequiredIf("IsComplex", " false")]
        public int? GstRateID { get; set; }
        [StringLength(10)]
        public string HsnCode { get; set; }
        [Required]
        public bool IsComplex { get; set; } = false;
        [Required]
        public string NatureOfAsset { get; set; } // Tangible, Intangible
        [RequiredIf("IsComplex", " false")]
        public DateTime? ProjectedDisposalDate { get; set; }
        [RequiredIf("IsComplex", " false")]
        public decimal? RateOfDepreciation { get; set; }
        [RequiredIf("IsComplex", " false")]
        public decimal? SalvageValue { get; set; }
        public bool Scrap { get; set; } = false;
        public decimal? SpanOfYear { get; set; }
        public decimal? TaxRate { get; set; }
        [RequiredIf("IsComplex", " false")]
        public DateTime? WarrentyDate { get; set; }
        public int RecordStatus { get; set; }
        //public decimal? TotalValue { get; set; }
        //[ExpressiveAnnotations.Attributes.RequiredIf("IsComplex == false")]
        public string SerialNumber { get; set; }
        [RequiredIf("IsComplex", " false")]
        public virtual PartyModel VendorInfo { get; set; }
        public string AssetStatus { get; set; }
    }

    public class RequiredIfComplexAndAssetStatusAttribute : ValidationAttribute
    {
        private readonly string _booleanPropertyName;
        private readonly string _booleanExpectedValue;
        private readonly string _stringPropertyName;
        private readonly string _stringExpectedValue;

        public RequiredIfComplexAndAssetStatusAttribute(string booleanPropertyName, string booleanExpectedValue, string stringPropertyName, string stringExpectedValue)
        {
            _booleanPropertyName = booleanPropertyName;
            _booleanExpectedValue = booleanExpectedValue;
            _stringPropertyName = stringPropertyName;
            _stringExpectedValue = stringExpectedValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the boolean property
            PropertyInfo booleanProperty = validationContext.ObjectType.GetProperty(_booleanPropertyName);
            if (booleanProperty == null)
                return new ValidationResult($"Property '{_booleanPropertyName}' not found.");

            bool booleanPropertyValue = (bool)booleanProperty.GetValue(validationContext.ObjectInstance);

            // Get the string property
            PropertyInfo stringProperty = validationContext.ObjectType.GetProperty(_stringPropertyName);
            if (stringProperty == null)
                return new ValidationResult($"Property '{_stringPropertyName}' not found.");

            string stringPropertyValue = (string)stringProperty.GetValue(validationContext.ObjectInstance);

            // Check the conditions
            if (booleanPropertyValue.ToString().Equals(_booleanExpectedValue, StringComparison.InvariantCultureIgnoreCase) &&
                stringPropertyValue.Equals(_stringExpectedValue, StringComparison.InvariantCultureIgnoreCase))
            {
                if (value == null)
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
                }
            }

            return ValidationResult.Success;
        }
    }
}