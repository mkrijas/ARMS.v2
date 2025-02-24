using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Model representing an inventory item
    public class InventoryItemModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InventoryItemModel>(Json);
        }
        public InventoryItemModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? InventoryItemID { get; set; } // Unique identifier for the inventory item
        //[Required]
        public virtual int? InventoryGroupID { get; set; }
        public string InventoryItemCode { get; set; }
        //[Required]
        public string ItemDescription { get; set; }
        //[Required]
        public virtual string UoM { get; set; }
        //[StringLength(8)]
        public virtual string HsnCode { get; set; }
        public virtual decimal? QtyAvailable { get; set; }
        public virtual InventoryGroupModel Group { get; set; } = new(); // Associated inventory group
        public virtual InventoryItemGroupModel ItemGroup { get; set; } = new(); // Associated item group
        public virtual SharedModels.UserInfoModel UserInfo { get; set; }
        public virtual string? Group2 { get; set; }
        public virtual string? Make { get; set; }
        public string? PartNumber { get; set; }
    }

    // Model representing an inventory group
    public class InventoryGroupModel :ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InventoryGroupModel>(Json);
        }
        public InventoryGroupModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? InventoryGroupID { get; set; } // Unique identifier for the inventory group
        [Required]
        public string InventoryGroupName { get; set; }
        public int? MappedPurchaseHead { get; set; } // Mapped purchase head ID
        public int? MappedConsumptionHead { get; set; } // Mapped consumption head ID
        public int? MappedNonInventoryPurchaseHead { get; set; } // Mapped non-inventory purchase head ID
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Model representing a second-level inventory group
    public class InventoryGroup2Model : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InventoryGroup2Model>(Json);
        }
        public InventoryGroup2Model()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? Group2ID { get; set; } // Unique identifier for the item group
        [Required]
        public string GroupDescription { get; set; }
        public int? Group1ID { get; set; }
        public virtual InventoryGroupModel Group { get; set; } = new();
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Model representing an inventory item group
    public class InventoryItemGroupModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InventoryItemGroupModel>(Json);
        }
        public InventoryItemGroupModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? ItemGroupID { get; set; } // Unique identifier for the item group
        [Required]
        public string ItemGroupDescription { get; set; }
        public int? Group2ID { get; set; }
        [Required]
        public string UoM { get; set; }
        [StringLength(8)]
        public string HsnCode { get; set; }
        public string Makes { get; set; }
        public List<InventoryItemModel> ItemMake { get; set; } = new(); // List of inventory items associated with the group
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Model representing a job card tracking entry
    public class JobCardTrackModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InventoryItemModel>(Json);
        }
        public JobCardTrackModel()
        {

            UserInfo = new SharedModels.UserInfoModel();
        }
        public DateTime? DocumentDate { get; set; }
        [Required]
        public decimal? Odometer { get; set; }
        public int? JobCardID { get; set; }
        [Required]
        public int? JobcardNumber { get; set; }
        [Required]
        public string UoM { get; set; }
        [StringLength(8)]
        public string HsnCode { get; set; }
        public virtual decimal? QtyAvailable { get; set; }
        public InventoryGroupModel Group { get; set; } = new();
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}