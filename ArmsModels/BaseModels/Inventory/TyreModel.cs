using ArmsModels.SharedModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static System.Formats.Asn1.AsnWriter;

namespace ArmsModels.BaseModels
{
    // Represents a tyre with various properties related to its identification and status
    public class TyreModel
    {
        public int? TyreID { get; set; } // Unique identifier for the tyre (nullable)
        public string ItemType { get; set; }
        [Required]
        public string TyreSerialNumber { get; set; }
        public int? BranchID { get; set; }
        public string Make { get; set; } // Hint: MRF , CEAT
        [Required]
        public int? InventoryItemID { get; set; }
        public string ItemCode { get; set; }
        [Required]
        public long? InventoryBatchID { get; set; }
        [RequiredIf("ItemType", "Tyre")]
        public string TyreType { get; set; } // Front/ Back/ All-Position
        [RequiredIf("ItemType", "Tyre")]
        public string TyreSize { get; set; } // 1000 x 25 etc
        public virtual string TyrePosition { get; set; }
        //[Required]
        public virtual int? TotalExpectedLife { get; set; }
        public bool? Tubeless { get; set; } = null;
        [RequiredIf("ItemType", "Tyre")]
        public int? TyreStatus { get; set; }
        public string LockText { get; set; }
        public string WarrantyCard { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
        public bool IsChecked { get; set; } = false;
        public bool IsMounted { get; set; } = false;
        // Property to get the status description based on TyreStatus
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
                    case < 0:
                        return "Scrap";
                    default:
                        return null;
                }
            }
        }

    }

    // Represents the position of a tyre on a vehicle
    public class TyrePositionModel
    {
        public int? PositionID { get; set; } // Unique identifier for the position (nullable)
        public string Side { get; set; } // LEFT , RIGHT
        public string Description { get; set; }
        public bool IsChecked { get; set; } = false;
        public bool IsMounted { get; set; } = false;
        public int? SRow { get; set; }
        public int? SColumn { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    // Maps tyre types to their positions
    public class TyreTypeAndPositionMappingModel
    {
        public int? ID { get; set; }
        public short? TruckTypeID { get; set; }
        public string PositionIDs { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    // Represents a mounted tyre with details about its installation
    public class TyreMountedModel
    {
        public int? ID { get; set; } // Unique identifier for the mounted tyre (nullable)
        [Required]
        public int? TyreID { get; set; }
        public string TyreNo { get; set; }
        [Required]
        public int? RequestID { get; set; }
        [Required]
        public int? TruckID { get; set; }        
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

    // Represents a tyre that has been unmounted
    public class TyreUnMountedModel
    {
        [Required]
        [ValidateComplexType]
        public TyreModel Tyre { get; set; } = new(); // Tyre information (required)
        [Required]
        public int? UnmountedKM { get; set; }
        public StoreModel Store { get; set; } // Store information associated with the unmounting(optional)
        public UserInfoModel UserInfo { get; set; } = new();
    }

    // Represents a request for tyre resole
    public class TyreResoleModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TyreResoleModel>(Json);
        }
        public int? ID { get; set; } // Unique identifier for the resole request (nullable)
        public PartyModel Party { get; set; } // Information about the party requesting the resole (optional)
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

    // Represents a tyre in a resole delivery
    public class ResoleDeliveryTyreModel
    {
        public int? ID { get; set; }
        public int? DeliveryID { get; set; }
        public int? TyreID { get; set; }
        public virtual bool TaxIncluded { get; set; }
        public virtual TyreModel Tyre { get; set; } // Tyre information associated with the delivery (optional
        public int? Status { get; set; } 
        [RequiredIf("Status", " 1")]
        public decimal? Amount { get; set; }
        [RequiredIf("TaxIncluded", " true")]

        public decimal? Tax { get; set; }
        public decimal? TDS { get; set; }
        public virtual decimal? GstRate { get; set; }
        public virtual decimal? TotalAmount { get; set; }
    }

    // Represents a delivery associated with a tyre resole request
    public class ResoleDeliveryModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ResoleDeliveryModel>(Json);
        }
        public int? ID { get; set; } // Unique identifier for the delivery (nullable)
        public int? ResoleID { get; set; }
        public int? BranchID { get; set; }
        public TyreResoleModel Resole { get; set; } // Resole information associated with the delivery (optional)
        public PartyModel Party { get; set; } // Information about the party receiving the delivery (optional)
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? RequestedDate { get; set; }
        [Required]
        public DateTime? DeliveryDate { get; set; }
        [Required]
        public string UsageCode { get; set; }
        public bool TaxIncluded { get; set; }
        public int? PID { get; set; } = 0;
        public List<int?> Tyres { get; set; }
        [Required]
        public List<ResoleDeliveryTyreModel> ResoleDeliveryTyreList = new(); // List of tyres in the delivery (required)
        public UserInfoModel UserInfo { get; set; } = new();
    }

    // Represents a tyre's kilometer reading
    public class TyreKmReadingModel
    {
        public int? ID { get; set; } // Unique identifier for the km reading (nullable)
        public TyreModel Tyre { get; set; } = new(); // Tyre information associated with the reading
        public string Title { get; set; }
        public int? KmReading { get; set; }
        public long? NotificationID { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    // Represents a swap operation between two tyres
    public class TyreSwapModel
    {
        public int? ID { get; set; } // Unique identifier for the swap operation (nullable)
        [Required]
        public int? TruckID { get; set; }
        [Required]
        public int? TyreA { get; set; }
        [Required]
        public int? TyreB { get; set; }
        [Required]
        public int? TyreATargetPosition { get; set; }
        [Required]
        public int? TyreBTargetPosition { get; set; }
        public int? TyreACurrentKM { get; set; }
        public int? TyreBCurrentKM { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class TyreHistoryModel
    {
        public string Title  { get; set; }
        public string Info { get; set; }
        public string Position { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? KmFrom { get; set; }
        public int? KmTo { get; set; }
        public int? RunKM { get; set; }
    }
}