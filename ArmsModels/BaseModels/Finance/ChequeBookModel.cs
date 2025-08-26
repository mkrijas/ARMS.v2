
using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Model representing a bank account
    public class ChequeBookModel
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ChequeBookModel>(Json);
        }
        public ChequeBookModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? ChequeBookID { get; set; } // Unique identifier for the bank account
        [Required]
        public int? OwnBankAccountID { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string StartingChequeNo { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string EndingChequeNo { get; set; }
        [Required]
        public DateTime? CreatedDate { get; set; } = DateTime.Today;
        public int? PendingLeaves { get; set; }
        public int? ActiveLeaves { get; set; }
        public int? CashedLeaves { get; set; }
        public int? CancelledLeaves { get; set; }
        public int? DeletedLeaves { get; set; }
        public int? AuthLevelID { get; set; }
        public string AuthStatus { get; set; }
        public string Remarks { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class ChequeBookLeavesModel
    {
        public ChequeBookLeavesModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? LeafID { get; set; } // Unique identifier for the bank account
        [Required]
        public int? ChequeBookID { get; set; }
        [StringLength(6, MinimumLength = 6)]
        public string ChequeNo { get; set; }
        public decimal? Amount { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}