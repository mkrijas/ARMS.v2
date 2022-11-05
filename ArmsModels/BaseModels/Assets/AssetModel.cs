using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.SharedModels;
using ExpressiveAnnotations.Attributes;


namespace ArmsModels.BaseModels
{

    public class AssetModel
    {
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
        public AssetClassModel SubClass { get; set; } // Printers,Chair,Engine etc
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public int? GstRateID { get; set; }
        [Required]
        [StringLength(8)]
        public string HsnCode { get; set; }
        [RequiredIf("IsComplex == false")]
        public DateTime? WarrentyDate { get; set; }
        [Required]
        public PartyModel VendorInfo { get; set; }
        [RequiredIf("IsComplex == false")]
        public string DepreciationBookCode { get; set; }// Income Tax,Company Act
        [RequiredIf("IsComplex == false")]
        public string DepreciationMethod { get; set; }// Straigt Line,Diminishing Balance,Sum of Years Digits 
        [RequiredIf("IsComplex == false")]
        public decimal? RateOfDepreciation { get; set; }
        [RequiredIf("IsComplex == false")]
        public DateTime? DepreciationStartingDate { get; set; }
        [RequiredIf("IsComplex == false")]
        public DateTime? DepreciationEndingDate { get; set; }
        [RequiredIf("IsComplex == false")]
        public decimal? BookValue { get; set; }
        public decimal? CurrentValue { get; set; }
        public decimal? TotalValue { get; set; }
        [RequiredIf("IsComplex == false")]
        public decimal? SpanOfYear { get; set; }
        [RequiredIf("IsComplex == false")]
        public decimal? SalvageValue { get; set; }        
        public virtual decimal? DepreciableValue
        {
            get
            {
                return BookValue - SalvageValue;
            }
        }
        [RequiredIf("IsComplex == false")]
        public DateTime? ProjectedDisposalDate { get; set; }
        public bool Scrap { get; set; } = false;
        public string Status { get; set; }//Scrap,Dismantled,Sold,Revaluated        
        public UserInfoModel UserInfo { get; set; } = new();
        //Additional Methods Required are 
        //Revaluation
        //Dismantle
        //Sale        
    }


    public class AssetClassModel
    {
        public int? AssetClassID { get; set; }
        [Required]
        public string AssetClassName { get; set; }
        [Required]
        public int? PostingGroupID { get; set; }
        public int? AssetSubClassID { get; set; }
        public virtual string AssetSubclass { get; set; }
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
        public string Title { get; set; }
        public int? AssetID { get; set; }
        public ChartOfAccountModel Capitalization { get; set; }
        public ChartOfAccountModel CWIP { get; set; }
        public ChartOfAccountModel AccummulatedDepreciation { get; set; }
        public ChartOfAccountModel Depreciation { get; set; }
        public ChartOfAccountModel Revaluation { get; set; }
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
