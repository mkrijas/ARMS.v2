using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
namespace ArmsModels.BaseModels
{
    public class OutstandingBillsModel : TransactionBaseModel
    {
        public int? BoID { get; set; }
        public PartyModel PartyInfo { get; set; }        
        public decimal? Amount { get; set; }
        public virtual string BranchName { get; set; }        
        public string ReferenceDocNo { get; set; }
        public DateTime? ReferenceDocDate { get; set; }
        public int? PaymentMemoID { get; set; }
        public int? SettlementID { get; set; }
        public int? AutoSettleID { get; set; }
        public OutstandingBillTableType BillInfo { get; set; }
   

    }
    public class OutstandingBillTableType
    {
        public int ? BoID { get; set; }
        public int ? MId { get; set; }
        public decimal ? InitialAmount { get; set; }
    }
}
