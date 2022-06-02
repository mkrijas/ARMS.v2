using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class InventoryGrnModel : InventoryBaseModel
    {
        public InventoryGrnModel(string grnNo, bool invoiced, bool _approved, UserInfoModel _approvedInfo) : base(_approved, _approvedInfo)
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
        public bool Invoiced { get; }       
    }


    public class PurchaseOrderModel : InventoryBaseModel
    {
        public PurchaseOrderModel()
        {
        }
        public PurchaseOrderModel(bool _grnCreated,string _poNo, bool _approved, UserInfoModel _approvedInfo) :base(_approved,_approvedInfo)
        {
            GrnCreated = _grnCreated;
            PONo = _poNo;            
        }

        public int? POID { get; set; }
        public string PONo { get; }
        public int? PRID { get; set; }
        public int? QuoteID { get; set; }
        public bool GrnCreated { get; }
    }



    public class InventoryBaseModel
    {
        public InventoryBaseModel()
        {
            UserInfo = new();
            Entries = new();
            ApprovedInfo = new();
        }      

        public InventoryBaseModel(bool _approved, UserInfoModel _approvedInfo)
        {
            UserInfo = new();
            Entries = new();            
            Approved = _approved;
            ApprovedInfo = _approvedInfo;
        }
        public int? StoreID { get; set; }
        [Required]
        public DateTime? EntryDate { get; set; }       
        [Required]
        public int? PartyBranchID { get; set; }
        public virtual string PartyName { get; set; }
        public decimal? TotalValue { get; set; }
        public string Reference { get; set; }
        public string Remarks { get; set; }        
        public bool Approved { get; }
        public UserInfoModel ApprovedInfo { get; }
        public UserInfoModel UserInfo { get; set; }
        [ValidateComplexType]
        public List<InventoryItemEntryModel> Entries { get; set; }
    }

    public class InventoryItemEntryModel
    {
        public long? ItemEntryID { get; set; }        
        [Required]
        public int? ItemID { get; set; }
        [Required]
        public decimal? ItemQty { get; set; }        
        public decimal? ItemRate { get; set; }
        public decimal? ItemGstVal { get; set; }
    }
}
