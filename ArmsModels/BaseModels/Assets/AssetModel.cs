using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.SharedModels;
using ExpressiveAnnotations.Attributes;
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
        public int? AssetID { get; set; }
        [Required]
        public string Description { get; set; }
        public int? ParentAssetID { get; set; }
        public bool IsComplex { get; set; } = false;

        [StringLength(8)]
        public virtual string AssetCode { get; set; }
        public string SerialNumber { get; set; }
        [Required]
        public string NatureOfAsset { get; set; }//Tangible, Intangible
        [Required]
        public AssetClassModel AssetClass { get; set; } // Building,Vehicle,Computer etc
        [Required]
        public AssetSubClassModel SubClass { get; set; } // Printers,Chair,Engine etc
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public int? GstRateID { get; set; }
        [Required]
        [StringLength(8)]
        public string HsnCode { get; set; }
        [ExpressiveAnnotations.Attributes.RequiredIf("IsComplex == false")]
        public DateTime? WarrentyDate { get; set; }
        [Required]
        public PartyModel VendorInfo { get; set; }
        [ExpressiveAnnotations.Attributes.RequiredIf("IsComplex == false")]
        public string DepreciationBookCode { get; set; }// Income Tax,Company Act
        [ExpressiveAnnotations.Attributes.RequiredIf("IsComplex == false")]
        public string DepreciationMethod { get; set; }// Straigt Line,Diminishing Balance,Sum of Years Digits 
        [ExpressiveAnnotations.Attributes.RequiredIf("IsComplex == false")]
        public decimal? RateOfDepreciation { get; set; }
        [ExpressiveAnnotations.Attributes.RequiredIf("IsComplex == false")]
        public DateTime? DepreciationStartingDate { get; set; }
        [ExpressiveAnnotations.Attributes.RequiredIf("IsComplex == false")]
        public DateTime? DepreciationEndingDate { get; set; }
        [ExpressiveAnnotations.Attributes.RequiredIf("IsComplex == false")]
        public decimal? BookValue { get; set; }
        public decimal? CurrentValue { get; set; }
        public decimal? TotalValue { get; set; }
        [ExpressiveAnnotations.Attributes.RequiredIf("IsComplex == false")]
        public decimal? SpanOfYear { get; set; }
        [ExpressiveAnnotations.Attributes.RequiredIf("IsComplex == false")]
        public decimal? SalvageValue { get; set; }
        public virtual decimal? DepreciableValue
        {
            get
            {
                return BookValue - SalvageValue;
            }
        }
        [ExpressiveAnnotations.Attributes.RequiredIf("IsComplex == false")]
        public DateTime? ProjectedDisposalDate { get; set; }
        public bool Scrap { get; set; } = false;
        public string Status { get; set; }//Scrap,Dismantled,Sold,Revaluated        
        public UserInfoModel UserInfo { get; set; } = new();
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
        [Required(ErrorMessage = "Name of Asset SubClass is required.")]
        public virtual string AssetSubclass { get; set; }
        [Required(ErrorMessage = "AssetClass is required.")]
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
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class AssetPostingGroupModel
    {
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
}