using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class TaxPurchaseModel
    {
        public int? PID { get; set; }
        public int? MID { get; set; }
        public int? GRNID { get; set; }
        public int? BranchID { get; set; }
        public PartyBranchModel PartyBranchInfo { get; set; }
        public bool IsCredit { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalTDS { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class TaxPurchaseExpensesModel
    {
        public long? TpeID { get; set; }
        public int? PID { get; set; }

        public int? BranchID { get; set; }
        public string UsageID { get; set; }
        public int? AccountID { get; set; }
        public decimal? Amount { get; set; }
        public decimal? SGST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? IGST { get; set; }
        public decimal? TDS { get; set; }
        public string Reference { get; set; }
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
    }

    public class TaxPurchaseItemModel
    {
        public long? TpiID { get; set; }
        public int? PID { get; set; }
        public int? ItemID { get; set; }
        public decimal? ItemRate { get; set; }
        public decimal? ItemQty { get; set; }
        public int? AccountID { get; set; }
        public decimal? Amount { get; set; }
        public decimal? SGST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? IGST { get; set; }

    }

}
