using ArmsModels.BaseModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Finance.Transactions
{
    public class SundryPaymentAssetModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<SundryPaymentAssetModel>(Json);
        }
        public int? SundryPaymentAssetID { get; set; }
        [Required]
        public AssetModel Asset { get; set; }
        public string PaymentMode { get; set; }
        [Required]
        public string PaymentArdCode { get; set; }
        //[Required]
        //public int? AssetCoaID { get; set; }
        [Required]
        public int? PaymentCoaID { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public string PaymentTool { get; set; }
        [RequiredIf("PaymentMode", "Bank")]
        public decimal? BankCharges { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public virtual string AccountName { get; set; }
        [Required]
        public string PayeeName { get; set; }
        [Required]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "PayeeContactNo must be 10 digits long")]
        public string PayeeContactNo { get; set; }
        public string Reference { get; set; }
        //public ArmsModels.SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
