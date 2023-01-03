using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace ArmsModels.BaseModels
{
    public class PartyPaymentMemoModel : TransactionBaseModel
    {
        public int? PaymentMemoID { get; set; }
        public int? PaymentInitiatedID { get; set; }
        public PartyModel PartyInfo { get; set; }

        public byte? PaymentStatus { get; set; } = 0; // 0 - generated; 1 - initiated; 2 - completed;
        public List<BillsPaidModel> Bills { get; set; }
    }


    //public class OutstandingBillsModel
    //{
    //    public int? BoID { get; set; }
    //    public string BillTransactionType { get; set; }
    //    public int? BillTransactionID { get; set; }
    //    public PartyModel PartyInfo { get; set; }
    //    public decimal? InitialAmount { get; set; }
    //    private decimal? _amount;
    //    public decimal? OutstandingAmount {
    //        get { return _amount; }
    //        set
    //        {
    //            _amount = value;
    //            if (value > 0)
    //                DrCr = "Dr";
    //            else
    //                DrCr = "Cr";
    //        } 
    //    }
    //    public string DrCr { get; private set; }
    //    public string DocNumber { get; set; }
    //    public virtual string BranchName { get; set; }
    //    public virtual int? BranchID { get; set; }
    //    public DateTime? DocDate { get; set; }
    //    public string InvoiceNumber { get; set; }
    //    public DateTime? InvoiceDate { get; set; }
    //}


    public class BillsPaidModel
    {
        decimal? _PayAmount;
        public int? BpID { get; set; }
        public int? BoID { get; set; }
        public virtual decimal? OutstandingAmount { get; set; }

        public decimal? PayAmount
        {
            get { return _PayAmount; }
            set
            {
                _PayAmount = (value > (OutstandingAmount ?? 0) ? OutstandingAmount : value);
            }
        }
        public virtual string BranchName { get; set; }
        public virtual int? BranchID { get; set; }
        public virtual string InvoiceNumber { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
    }

    public class PaymentInitiatedModel
    {
        public PaymentInitiatedModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? PiID { get; set; }
        public DateTime? DueOn { get; set; }
        public int? BranchID { get; set; }
        public int? AuthLevelId { get; set; }
        public string AuthStatus { get; set; }
        public DateTime? DocDate { get; set; }
        public string DocNumber { get; set; }
        public decimal? TotalAmount { get; set; }
        public List<PartyPaymentMemoModel> PaymentMemos { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class PaymentFinishModel
    {
        public PaymentFinishModel()
        {
            UserInfo = new SharedModels.UserInfoModel();

        }
        public int? PfID { get; set; }
        public int? PiID { get; set; }
        public string PaymentMode { get; set; }      // Bank,Cash   
        public string PaymentTool { get; set; } // Cheque,DD
        public string PaymentArdCode { get; set; }
        public decimal? BankCharges { get; set; }
        public int? PaymentCoaID { get; set; }
        public DateTime? DocumentDate { get; set; }
        [Required]
        public int? BranchID { get; set; }
        public int? AuthLevelId { get; set; }
        public string AuthStatus { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Narration { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        public List<PaymentEntryModel> Payments { get; set; }
    }


    public class PaymentEntryModel
    {
        public int? PeID { get; set; }
        public int? PaymentMemoID { get; set; }
        public virtual string DocumentNumber { get; set; }
        public string Reference { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? Amount { get; set; }
    }
  
}
