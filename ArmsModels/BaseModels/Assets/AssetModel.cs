using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.SharedModels;


namespace ArmsModels.BaseModels
{
    public class AssetHolderModel
    {
        public int? AssetHolderID { get; set; } 
        [Required]
        public string Description { get; set; }
        [Required]
        public int? BranchID { get; set; }
        public List<AssetModel> Assets { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();

    }


    public class AssetModel
    {
        public int? AssetID { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [StringLength(8)]
        public string AssetCode { get; set; }
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
        public DateTime? WarrentyDate { get; set; }
        [Required]
        public PartyModel VendorInfo { get; set; }
        public string DepreciationBookCode { get; set; }// Income Tax,Company Act
        public string DepreciationMethod { get; set; }// Straigt Line,Diminishing Balance,Sum of Years Digits
        public decimal? RateOfDepreciation { get; set; }
        public DateTime? DepreciationStartingDate { get; set; }
        public DateTime? DepreciationEndingDate { get; set; }
        [Required]
        public decimal? BookValue { get; set; }
        [Required]
        public decimal? SpanOfYear { get; set; }
        [Required]
        public decimal? SalvageValue { get; set; }
        public virtual decimal? DepreciableValue
        {
            get
            {
                return BookValue - SalvageValue;
            }
        }
        public DateTime? ProjectedDisposalDate { get; set; }
        public string Status { get; set; }//Scrap,Dismantled,Sold,Revaluated

        public int? AccountTransactionID { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
        //Additional Methods Required are 
        //Revaluation
        //Dismantle
        //Sale
        //Scrap
    }


    public class AssetClassModel
    {
        public int? AssetClassID { get; set; }
        [Required]
        public string AssetClassName { get; set; }
        public int? ParentID { get; set; }
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
        [Required]
        public decimal? Amount { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }
   
}
