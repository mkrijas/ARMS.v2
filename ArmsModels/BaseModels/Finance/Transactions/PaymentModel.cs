using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels.Finance.Transactions
{
    public class PartyPaymentMemoModel :TransactionBaseModel
    {
        public int? PaymentMemoID { get; set; }
        public PartyBranchModel PartyBranchInfo { get; set; }
        public byte PaymentStatus { get; set; } = 0; // 0 - generated; 1 - initiated; 2 - completed;
        public List<BillsPaidModel> Bills { get; set; }
    }


    public class OutstandingBillsModel
    {
        public int? BoID { get; set; }
        public string BillTransactionType { get; set; }
        public int? BillTransactionID { get; set; }
        public PartyBranchModel PartyBranchInfo { get; set; }
        public decimal? InitialAmount { get; set; }
        public decimal? OutstandingAmount { get; set; }
        public string DocNumber { get; set; }
        public DateTime? DocDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
    }


    public class BillsPaidModel
    {
        public int? BpID { get; set; }
        public int? BoID { get; set; }
        public decimal? PayAmount { get; set; }
    }

    public class PaymentInitiatedModel  
    {
        public int? PiID { get; set; }
        public DateTime? DueOn { get; set; }
        public List<PartyPaymentMemoModel>  PaymentMemos { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class PaymentCompletedModel : TransactionBaseModel
    {
        public int? PcID { get; set; }
        public int? PiID { get; set; }        
        public string PaymentMode { get; set; }
        public int? CoaID { get; set; }
        public List<PaymentSubModel> Payments { get; set; }
    }


    public class PaymentSubModel
    {
        public int? PsID { get; set; }
        public int? PaymentMemoID { get; set; }
        public string Reference { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? Amount { get; set; }

    }

}
