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
        public InventoryGrnModel(string grnNo, bool invoiced )
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
        public PurchaseOrderModel(bool _grnCreated,string _poNo) 
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
        }

        public int? InvTranID { get; set; }
        public int? StoreID { get; set; }
        [Required]
        public DateTime? EntryDate { get; set; }       
        [Required]
        public int? PartyID { get; set; }
        public string PartyCode { get; set; }
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
