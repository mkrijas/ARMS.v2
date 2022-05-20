using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels.Inventory
{
    public class InventoryGrnModel : InventoryBaseModel
    {    
        public int? GrnID { get; set; }
        public string GrnNo { get; }        
        public int? POID { get; set; }      
        public bool Invoiced { get; }       
    }


    public class PuchaseOrderModel : InventoryBaseModel
    {
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
            Approved = new();
        }      
        public int? StoreID { get; set; }
        [Required]
        public DateTime? EntryDate { get; set; }       
        [Required]
        public int? PartyBranchID { get; set; }
        public string Reference { get; set; }
        public string Remarks { get; set; }        
        public bool Approved { get; set; }
        public UserInfoModel ApprovedInfo { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public List<InventoryItemEntryModel> Entries { get; set; }
    }

    public class InventoryItemEntryModel
    {
        public long? ItemEntryID { get; set; }
        public int? ParentID { get; set; }
        [Required]
        public int? ItemID { get; set; }
        [Required]
        public decimal? ItemQty { get; set; }        
        public decimal? ItemRate { get; set; }
        public decimal? ItemGstVal { get; set; }
    }
}
