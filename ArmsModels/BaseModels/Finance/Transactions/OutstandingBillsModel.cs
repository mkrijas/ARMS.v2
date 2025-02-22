using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
namespace ArmsModels.BaseModels
{
    public class OutstandingBillsModel : TransactionBaseModel
    {
        // Model representing outstanding bills
        public OutstandingBillsModel()
        {
            PartyInfo = new();
        }
        public virtual int? BoID { get; set; } // Unique identifier for the outstanding bill
        public PartyModel PartyInfo { get; set; } // Information about the party associated with the outstanding bill
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
        public string SubArdCode { get; set; }
    }

    // Model representing an auto-settlement transaction
    public class AutoSettleModel : TransactionBaseModel,ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<AutoSettleModel>(Json);
        }
        public int? AutoSettleID { get; set; } // Unique identifier for the auto-settlement
        public PartyModel PartyInfo { get; set; } = new(); // Information about the party associated with the auto-settlement
        public List<BillsPaidModel> Bills { get; set; } = new(); // List of bills that have been paid in the auto-settlement
    }
}
