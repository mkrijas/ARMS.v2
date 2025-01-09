using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class TyreModel
    {
        public int? TyreID { get; set; }
        [Required]
        public string TyreSerialNumber { get; set; }
        public int? BranchID { get; set; }
        public string Make { get; set; } // Hint: MRF , CEAT
        [Required]
        public int? InventoryItemID { get; set; }
        [Required]
        public long? InventoryBatchID { get; set; }
        [Required]
        public string TyreType { get; set; } // Front/ Back/ All-Position
        [Required]
        public string TyreSize { get; set; } // 1000 x 25 etc
        public virtual string TyrePosition { get; set; }
        [Required]
        public virtual int? TotalExpectedLife { get; set; }
        public bool Tubeless { get; set; } = false;
        public byte? TyreStatus { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
        public bool IsChecked { get; set; } = false;
        public bool IsMounted { get; set; } = false;
        public string Status
        {
            get
            {
                switch (TyreStatus)
                {
                    case 0:
                        return "New";
                    case 1:
                        return "1st Resole";
                    case 2:
                        return "2nd Resole";
                    case 3:
                        return "3rd Resole";
                    case 99:
                        return "Scrap";
                    default:
                        return null;
                }
            }
        }
    }

    public class TyrePositionModel
    {
        public int? PositionID { get; set; }
        public string Side { get; set; } // LEFT , RIGHT
        public string Description { get; set; }
        public bool IsChecked { get; set; } = false;
        public bool IsMounted { get; set; } = false;
        public int? SRow { get; set; }
        public int? SColumn { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class TyreTypeAndPositionMappingModel
    {
        public int? ID { get; set; }
        public short? TruckTypeID { get; set; }
        public string PositionIDs { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class TyreMountedModel
    {
        public int? ID { get; set; }
        [Required]
        public int? TyreID { get; set; }
        public string TyreNo { get; set; }
        [Required]
        public int? RequestID { get; set; }
        [Required]
        public int? TruckID { get; set; }
        [Required]
        public int? PositionID { get; set; } // TyrePosition Table 
        public string PositionName { get; set; }
        [Required]
        public DateTime? MountedOn { get; set; }
        public DateTime? UnmountedOn { get; set; }
        [Required]
        public int? MountedKM { get; set; }
        public int? UnmountedKM { get; set; }
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        public int? RunKM
        {
            get { return UnmountedKM - MountedKM; }
        }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class TyreUnMountedModel
    {
        [Required]
        [ValidateComplexType]
        public TyreModel Tyre { get; set; } = new();
        [Required]
        public int? UnmountedKM { get; set; }
        public StoreModel Store { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class TyreResoleModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TyreResoleModel>(Json);
        }
        public int? ID { get; set; }
        public PartyModel Party { get; set; }
        [Required]
        public int? PartyID { get; set; }
        [Required]
        public DateTime? RequestedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public virtual int? NoOfTyres { get; set; }
        public int? BranchID { get; set; }
        public int? DeliveryID { get; set; }
        [Required(ErrorMessage = "The tyre is required.")]
        [MinLength(1, ErrorMessage = "At least 1 tyre should be selected.")]
        public List<int?> Tyres { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class ResoleDeliveryTyreModel
    {
        public int? ID { get; set; }
        public int? DeliveryID { get; set; }
        public int? TyreID { get; set; }
        public bool TaxIncluded { get; set; }
        public TyreModel Tyre { get; set; }
        public byte? Status { get; set; } 
        [RequiredIf("Status", " 1")]
        public decimal? Amount { get; set; }
        [RequiredIf("TaxIncluded", " true")]

        public decimal? Tax { get; set; }
        public decimal? TotalAmount { get; set; }
    }

    public class ResoleDeliveryModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ResoleDeliveryModel>(Json);
        }
        public int? ID { get; set; }
        public int? ResoleID { get; set; }
        public int? BranchID { get; set; }
        public TyreResoleModel Resole { get; set; }
        public PartyModel Party { get; set; }
        public DateTime? RequestedDate { get; set; }
        [Required]
        public DateTime? DeliveryDate { get; set; }
        [Required]
        public string UsageCode { get; set; }
        public bool TaxIncluded { get; set; }
        public int? PID { get; set; } = 0;
        public List<int?> Tyres { get; set; }
        [Required]
        public List<ResoleDeliveryTyreModel> ResoleDeliveryTyreList = new();
        public UserInfoModel UserInfo { get; set; } = new();
    }
}