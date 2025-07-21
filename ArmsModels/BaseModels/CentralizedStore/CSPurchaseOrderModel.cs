using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    // Model representing an inventory Goods Receipt Note (GRN)
    //public class CSGrnModel : CSOrderBaseModel, ICloneable
    //{
    //    public object Clone()
    //    {
    //        string Json = JsonConvert.SerializeObject(this);
    //        return JsonConvert.DeserializeObject<CSGrnModel>(Json);
    //    }
    //    public CSGrnModel(string grnNo, bool invoiced)
    //    {
    //        GrnNo = grnNo;
    //        Invoiced = invoiced;
    //    }
    //    public CSGrnModel()
    //    {

    //    }
    //    public int? GrnID { get; set; } // Unique identifier for the GRN
    //    public string GrnNo { get; }
    //    public int? POID { get; set; }
    //    public bool Invoiced { get; }
    //    public decimal? IssuedQty { get; set; }
    //    public int? NoOfGR { get; set; }
    //    [Required]
    //    [ValidateComplexType]
    //    public BranchModel Branch { get; set; } // Store associated with the GRN
    //}

    // Model representing a purchase order
    public class CSPurchaseOrderModel : CSOrderBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CSPurchaseOrderModel>(Json);
        }
        public CSPurchaseOrderModel()
        {

        }
        public CSPurchaseOrderModel(bool _grnCreated, string _poNo)
        {
            GrnCreated = _grnCreated;
            PONo = _poNo;
        }
        public int? POID { get; set; } // Unique identifier for the purchase order
        public string PONo { get; }
        public bool GrnCreated { get; }
        [Required]
        [ValidateComplexType]
        public BranchModel Branch { get; set; }
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

    //// Model representing a purchase request
    //public class PurchaseRequestModel : InventoryBaseModel
    //{
    //    public PurchaseRequestModel() { }
    //    public int PrID { get; set; }
    //    public string PrNo { get; set; }
    //}

    // Base model for inventory transactions
    public class CSBaseModel
    {
        public CSBaseModel() { }
        public int? InvTranID { get; set; }
        public int? BranchID { get; set; }
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
        public List<CSItemEntryModel> Entries { get; set; } = new(); // List of inventory item entries
    }

    // Base model for inventory orders
    public class CSOrderBaseModel : CSBaseModel
    {
        public CSOrderBaseModel()
        {
            UserInfo = new();
            Entries = new();
        }
        [RequiredIf("UsedInventory", 0)]
        public int? PartyID { get; set; }
        public string PartyCode { get; set; }
        public int? UsedInventory { get; set; }
    }

    // Model representing an entry for an inventory item
    public class CSItemEntryModel
    {
        public long? ItemEntryID { get; set; } // Unique identifier for the item entry
        public long? RefID { get; set; }
        [Required]
        public int? ItemID { get; set; }
        public virtual int? CoaID { get; set; }
        public virtual string ItemCode { get; set; }
        public virtual string ItemDescription { get; set; }
        public virtual string ItemGroupDescription { get; set; }
        public virtual string PartNumber { get; set; }
        [Required]
        public decimal ItemQty { get; set; }
        public virtual string UOM { get; set; }
        public decimal? ItemRate { get; set; }
        public decimal? ItemGstVal { get; set; }
    }
}