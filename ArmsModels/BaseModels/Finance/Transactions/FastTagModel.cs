using ArmsModels.BaseModels;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace Core.BaseModels.Finance.Transactions
{
    public class FastTagTollModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<SundryPaymentModel>(Json);
        }
        [Required]
        public string PaymentMode { get; set; }
        [Required]
        public string PaymentArdCode { get; set; }
        [Required]
        public int? PaymentCoaID { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public string PaymentTool { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public decimal? BankCharges { get; set; }
        public virtual string AccountName { get; set; }
        [ValidateComplexType]
        [MustContain(ErrorMessage = "No items added for payment!")]
        public List<FastTagModel> FastTagModelList { get; set; } = new();

    }

    public class FastTagModel
    {
        public DateTime? TransactionDateTime { get; set; }
        public DateTime? ProcessedDateTime { get; set; }
        public int? BranchID { get; set; }
        public string BranchName { get; set; }
        public string NumberPlate { get; set; }
        public TripModel Trip { get; set; }
        public int TagAccountNumber { get; set; }
        public string Group { get; set; }
        public string Activity { get; set; }
        public string PlazaCode { get; set; }
        public string Description { get; set; }
        public string TransactionID { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DebitAmount { get; set; }
    }

    public class FastTagList
    {
        public DateTime? TransactionDateTime { get; set; }
        public string NumberPlate { get; set; }
    }
}