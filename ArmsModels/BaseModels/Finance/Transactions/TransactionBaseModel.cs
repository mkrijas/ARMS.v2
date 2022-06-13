using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class TransactionBaseModel
    {
        public TransactionBaseModel()
        {
            UserInfo = new();
            ApprovedInfo = new();
        }
        public int? MID { get; set; }
        [Required]
        public DateTime? DocumentDate { get; set; }
        public string DocumentNumber { get; set; }
        [Required]
        public int? BranchID { get; set; }
        public decimal? TotalAmount { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        public SharedModels.UserInfoModel ApprovedInfo { get; set; }
    }
}
