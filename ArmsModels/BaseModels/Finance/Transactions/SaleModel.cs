using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class SaleModel : TransactionBaseModel
    {
        public int? SID { get; set; } 
        public PartyModel PartyInfo { get; set; }  
        [Required]
        public bool IsCredit { get; set; }

        public decimal? AdditionalTCS { get; set; } 

        [ValidateComplexType]
        public List<SaleExpenseModel> Particulars { get; set; } = new();
        [ValidateComplexType]
        public List<SaleItemModel> Items { get; set; } = new();
    }    

    public class SaleExpenseModel
    {
        public long? TpeID { get; set; }
        public int? PID { get; set; }
        public decimal? GstRate { get; set; }
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public string UsageCode { get; set; }
        public int? CoaID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public decimal? SGST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? IGST { get; set; }
        public decimal? TCS { get; set; }
        public string BillReference { get; set; }
       
    }

    public class SaleItemModel
    {
        public long? TpiID { get; set; }
        public int? PID { get; set; }
        public decimal ? GstRate { get; set; }
        public int? ItemID { get; set; }
        public decimal? ItemRate { get; set; }
        public decimal? ItemQty { get; set; }
        public int? CoaID { get; set; }
        public decimal? Amount { get; set; }
        public decimal? SGST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? IGST { get; set; }
        public decimal? TCS { get; set; }
    }

   
}
