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
        public OutstandingBillsModel()
        {
            PartyInfo = new();
        }
        public virtual int? BoID { get; set; }        
        public PartyModel PartyInfo { get; set; }        
        public decimal? OutstandingAmount { get; set; }
        public virtual string OutstandingAmountDisplayText { get { return Math.Abs(OutstandingAmount.Value).ToString() + " " + (OutstandingAmount.Value < 0 ? "Cr" : "Dr"); } }
        public virtual string BranchName { get; set; }        
        public virtual string ReferenceDocNo { get; set; }
        public virtual DateTime? ReferenceDocDate { get; set; }
        public virtual int? PaymentMemoID { get; set; }
        public virtual int? SettlementID { get; set; }
        public virtual int? AutoSettleID { get; set; } 
        public bool isMemo { get; set; }
        public int? CoaID { get; set; }
    }   
}
