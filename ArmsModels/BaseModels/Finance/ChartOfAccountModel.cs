using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;

namespace ArmsModels.BaseModels
{
    public class ChartOfAccountModel
    {
        public ChartOfAccountModel()
        {
            UserInfo = new();
        }

        [Required]
        public int? CoaID { get; set; }
        [Required]
        public int? ParentID { get; set; }
        public string AccountCode { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "LedgerName is required.")]
        public string AccountName { get; set; }
        public string AccountDescription { get; set; }
        public string AccountType { get; set; }
        public bool SummaryAccount { get; set; }
        public bool LimitToPeriod { get; set; }
        [RequiredIfTrue("LimitToPeriod", ErrorMessage = "Select the Start date")]
        public DateTime? PeriodFrom { get; set; }
        [RequiredIfTrue("LimitToPeriod", ErrorMessage = "Select the End date")]
        public DateTime? PeriodTo { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class CoaBranchAvailabilityModel
    {
        public CoaBranchAvailabilityModel()
        {
            UserInfo = new();
        }
        private int? _id;
        public int? ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                IsSelected = value != null;
            }
        }
        public int? CoaID { get; set; }
        public string AccountName { get; set; }
        public int? BranchID { get; set; }
        public string BranchName { get; set; }
        public bool IsSelected { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // For selecting ARD codes in  transaction pages
    public class PaymentCodeModel
    {
        public int? ID { get; set; }
        public string ArdCode { get; set; }
        public string Title { get; set; }
        public int? CoaID { get; set; }
        public virtual string paymentMode { get; set; }
        public virtual decimal? BankCharges { get; set; }
        public virtual string BankTools { get; set; }
    }
}