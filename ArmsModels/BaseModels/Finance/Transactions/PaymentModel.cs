using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Runtime.ConstrainedExecution;

namespace ArmsModels.BaseModels
{
    public class PaymentMemoModel : TransactionBaseModel
    {
        public int? PaymentMemoID { get; set; }
        [Required]
        public string BusinessNature { get; set;} // "Suppier,Customer,Renter"
        [Required]
        public PartyModel PartyInfo { get; set; }        
        public int? PartyCoaID { get; set; }
        public string Reference { get; set; }
        [Required]
        public byte? PaymentStatus { get; set; } = 0; // 0 - generated; 1 - initiated; 2 - completed;
        public List<BillsPaidModel> Bills { get; set; } = new();
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
        public int? MID { get; set; }
        public bool IsMemo { get; set; } = false;
        public virtual decimal? OutstandingAmount { get; set; }        
        public decimal? PayAmount
        {
            get { return _PayAmount; }
            set
            {
               // _PayAmount = (Math.Abs(value??0) > Math.Abs(OutstandingAmount ?? 0) || Math.Abs(value + OutstandingAmount??0) > Math.Abs(OutstandingAmount??0) ? -OutstandingAmount : value);
            _PayAmount = value;
            }
        }
        public virtual string BranchName { get; set; }
        public virtual int? BranchID { get; set; }
        public virtual string InvoiceNumber { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }       
    }

    public class PaymentInitiatedModel : PaymentMemoModel
    {  
        public PaymentInitiatedModel(PaymentMemoModel ToCopy)
        {            
                Type type = typeof(PaymentMemoModel);
               
                foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    object Value = type.GetProperty(pi.Name).GetValue(ToCopy, null);
                if (Value != null)
                    this.GetType().GetProperty(pi.Name).SetValue(this, Value, null);                                 
                } 
        }

        public PaymentInitiatedModel()
        {

        }
        public int? PaymentInitiatedID { get; set; }
        [Required]
        public DateTime? DueOn { get; set; }
        [Required]
        public DateTime? InitiatedDocumentDate { get; set; } = DateTime.Today;        
    }

    public class PaymentFinishModel :PaymentInitiatedModel
    {       
        public PaymentFinishModel()
        {

        }
        public PaymentFinishModel(PaymentMemoModel ToCopy)
        {
            Type type = typeof(PaymentMemoModel);

            foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                object Value = type.GetProperty(pi.Name).GetValue(ToCopy, null);
                if (Value != null)
                this.GetType().GetProperty(pi.Name).SetValue(this, Value, null);
            }
        }
        public PaymentFinishModel(PaymentInitiatedModel ToCopy)
        {
            Type type = typeof(PaymentMemoModel);

            foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                object Value = type.GetProperty(pi.Name).GetValue(ToCopy, null);
                if (Value != null)
                    this.GetType().GetProperty(pi.Name).SetValue(this, Value, null);
            }
        }

        public int? PaymentFinalizeID { get; set; }       
        [Required]
        public string PaymentMode { get; set; }  // Bank,Cash   
        public string PaymentTool { get; set; }  // Cheque,DD
        [Required]
        public string PaymentArdCode { get; set; }
        public decimal? BankCharges { get; set; }
        [Required]
        public int? PaymentCoaID { get; set; }
        [Required]
        public DateTime? EffectiveDate { get; set; } = DateTime.Today;       
    }

}
