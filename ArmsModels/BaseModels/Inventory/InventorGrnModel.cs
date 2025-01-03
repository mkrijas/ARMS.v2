using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class InventoryGrnModel : InventoryOrderBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InventoryGrnModel>(Json);
        }
        public InventoryGrnModel(string grnNo, bool invoiced)
        {
            GrnNo = grnNo;
            Invoiced = invoiced;
        }
        public InventoryGrnModel()
        {

        }
        public int? GrnID { get; set; }
        public string GrnNo { get; }
        public int? POID { get; set; }
        [RequiredIf("UsedInventory",1)]
        public int? TruckID { get; set; }        
        public bool Invoiced { get; }
        [Required]
        [ValidateComplexType]
        public StoreModel Store { get; set; }
    }

    public class PurchaseOrderModel : InventoryOrderBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<PurchaseOrderModel>(Json);
        }
        public PurchaseOrderModel()
        {

        }
        public PurchaseOrderModel(bool _grnCreated, string _poNo)
        {
            GrnCreated = _grnCreated;
            PONo = _poNo;
        }
        public int? POID { get; set; }
        public string PONo { get; }
        public int? PRID { get; set; }
        public int? QuoteID { get; set; }
        public bool GrnCreated { get; }
        [Required]
        [ValidateComplexType]
        public StoreModel Store { get; set; }
        public string Status
        {
            get
            {
                switch (UserInfo.RecordStatus)
                {
                    case 3:
                        return "Available";
                    case 9:
                        return "Cancelled";
                    default:
                        return null;
                }
            }
        }
    }

    public class PurchaseRequestModel : InventoryBaseModel
    {
        public PurchaseRequestModel() { }
        public int PrID { get; set; }
        public string PrNo { get; set; }
    }

    public class InventoryBaseModel
    {
        public InventoryBaseModel() { }
        public int? InvTranID { get; set; }
        public int? StoreID { get; set; }
        [Required]
        public DateTime? EntryDate { get; set; }
        public bool Direction { get; set; } = true; // True for Inward, False for Outward;       
        public virtual string PartyName { get; set; }
        public decimal? TotalValue { get; set; }
        public string Reference { get; set; }
        public int? AuthLevelID { get; set; }
        public string AuthStatus { get; set; }
        public string Remarks { get; set; }
        public UserInfoModel UserInfo { get; set; }
        [ValidateComplexType]
        public List<InventoryItemEntryModel> Entries { get; set; } = new();
    }

    public class InventoryOrderBaseModel : InventoryBaseModel
    {
        public InventoryOrderBaseModel()
        {
            UserInfo = new();
            Entries = new();
        }
        [RequiredIf("UsedInventory",0)]
        public int? PartyID { get; set; }
        public string PartyCode { get; set; }
        public int? UsedInventory { get; set; }
    }

    public class InventoryItemEntryModel
    {
        public long? ItemEntryID { get; set; }
        public long? RefID { get; set; }
        [Required]
        public int? ItemID { get; set; }
        public virtual int? CoaID { get; set; }
        public virtual string ItemCode { get; set; }
        public virtual string ItemDescription { get; set; }
        public virtual string ItemGroupDescription { get; set; }
        [Required]
        public decimal ItemQty { get; set; }
        public virtual string UOM { get; set; }
        public decimal? ItemRate { get; set; }
        public decimal? ItemGstVal { get; set; }
    }
}