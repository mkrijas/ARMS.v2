using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
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
            return JsonConvert.DeserializeObject<FastTagTollModel>(Json);
        }
        public int? FastTagUploadID { get; set; }
        public string ProcessDocumentNumber { get; set; }
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
        public decimal? TotalAmount {  get; set; }
        public List<FastTagModel> FastTagModelList { get; set; } = new();
    }

    public class FastTagModel
    {
        public int? FastTagTollID { get; set; }
        public DateTime? TransactionDateTime { get; set; }
        //public DateTime? ProcessedDateTime { get; set; }
        public int? BranchID { get; set; }
        public int? TruckID { get; set; }
        public long? TripID { get; set; }
        public long? TripNumber { get; set; }
        public string PlazaCode { get; set; }
        public string Description { get; set; }
        public string TransactionID { get; set; }
        public bool Reimbursable {  get; set; }
        public decimal DebitAmount { get; set; }
        public virtual string BranchName { get; set; }
        public virtual string NumberPlate { get; set; }
        public virtual string TripPrefix { get; set; }
        public virtual string TripNumberDisplay { get { return TripPrefix + TripNumber.ToString().PadLeft(4, '0'); } }
        //public int TagAccountNumber { get; set; }
        //public string Group { get; set; }
        //public string Activity { get; set; }
        public virtual decimal CreditAmount { get; set; }
        public virtual byte? RecordStatus { get; set; }
    }

    public class FastTagList
    {
        public DateTime? TransactionDateTime { get; set; }
        public string NumberPlate { get; set; }
    }
}