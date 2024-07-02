using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
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
        public int? InventoryItemID { get; set; }
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
        public virtual InventoryGroupModel Group { get; set; } = new();
        public virtual InventoryItemGroupModel ItemGroup { get; set; } = new();
        public virtual SharedModels.UserInfoModel UserInfo { get; set; }
        public virtual string? Group2 { get; set; }
        public virtual string? Make { get; set; }
        public string? PartNumber { get; set; }
    }

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
        public int? InventoryGroupID { get; set; }
        [Required]
        public string InventoryGroupName { get; set; }
        public int? MappedPurchaseHead { get; set; }
        public int? MappedConsumptionHead { get; set; }
        public int? MappedNonInventoryPurchaseHead { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

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
        public int? Group2ID { get; set; }
        [Required]
        public string GroupDescription { get; set; }
        public int? Group1ID { get; set; }
        public virtual InventoryGroupModel Group { get; set; } = new();
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

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
        public int? ItemGroupID { get; set; }
        [Required]
        public string ItemGroupDescription { get; set; }
        public int? Group2ID { get; set; }
        [Required]
        public string UoM { get; set; }
        [StringLength(8)]
        public string HsnCode { get; set; }
        public string Makes { get; set; }
        public List<InventoryItemModel> ItemMake { get; set; } = new();
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

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