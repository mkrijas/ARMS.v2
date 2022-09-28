using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class TaxPurchaseModel : TransactionBaseModel
    {
        public int? PID { get; set; }        
        public int? GRNID { get; set; }        
        public PartyBranchModel PartyBranchInfo { get; set; }  
        [Required]
        public bool IsCredit { get; set; }
        [Required]
        public string InvoiceNo { get; set; }
        [Required]
        public DateTime? InvoiceDate { get; set; }        
        public decimal? AdditionalTDS { get; set; }  
        [Required]
        public bool NonStoreInventory { get; set; }
        [ValidateComplexType]
        public List<TaxPurchaseExpenseModel> Expenses { get; set; } = new();
        [ValidateComplexType]
        public List<TaxPurchaseItemModel> Items { get; set; } = new();
    }    

    public class TaxPurchaseExpenseModel
    {
        public long? TpeID { get; set; }
        public int? PID { get; set; }
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public string UsageID { get; set; }
        public int? CoaID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public decimal? SGST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? IGST { get; set; }
        public decimal? TDS { get; set; }
        public string BillReference { get; set; }
       
    }

    public class TaxPurchaseItemModel
    {
        public long? TpiID { get; set; }
        public int? PID { get; set; }
        public int? ItemID { get; set; }
        public decimal? ItemRate { get; set; }
        public decimal? ItemQty { get; set; }
        public int? CoaID { get; set; }
        public decimal? Amount { get; set; }
        public decimal? SGST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? IGST { get; set; }
        public decimal? TDS { get; set; }
    }

   
}
