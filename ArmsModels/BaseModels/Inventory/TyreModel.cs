using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class TyreModel
    {
        public int? TyreID { get; set; }
        public string TyreSerialNumber { get; set; }
        public int? BranchID { get; set; }
        public string Make { get; set; } // Hint: MRF , CEAT
        public int? InventoryItemID { get; set; }
        public int? InventoryBatchID { get; set; }
        [Required]
        public string TyreType { get; set; } // Front/ Back/ All-Position
        [Required]
        public string TyreSize { get; set; } // 1000 x 25 etc
        public bool Tubeless { get; set; } = false;
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class TyrePositionModel
    {
        public int? PositionID { get; set; }
        public string Side { get; set; } // LEFT , RIGHT
        public string Description { get; set; }
        public bool IsChecked { get; set; } = false;
        public int? SRow { get; set; }
        public int? SColumn { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class TyreTypeAndPositionMappingModel
    {
        public int? ID { get; set; }
        public int? TruckTypeID { get; set; }
        public string? PositionIDs { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class TyreMountedModel
    {
        public int? ID { get; set; }
        [Required]
        public int? TyreID { get; set; }
        [Required]
        public int? TruckID { get; set; }
        [Required]
        public int? PositionID { get; set; } // TyrePosition Table 
        [Required]
        public DateTime? MountedOn { get; set; }
        public DateTime? UnmountedOn { get; set; }
        [Required]
        public int? MountedKM { get; set; }
        public int? UnmountedKM { get; set; }
        public int? RunKM
        {
            get { return UnmountedKM - MountedKM; }
        }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class TyreResoleModel
    {
        public int? ID { get; set; }
        public PartyModel Party { get; set; }
        public DateTime? RequestedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public List<int?> Tyres { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }


}

