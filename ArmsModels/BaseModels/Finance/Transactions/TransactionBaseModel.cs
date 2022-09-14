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
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        public string Narration { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        public SharedModels.UserInfoModel ApprovedInfo { get; set; }
    }



    public class AccountInfoViewModel
    {
        public DateTime? DocumentDate { get; set; }
        public string DocumentNumber { get; set; }      
        public string CostCenter { get; set; }
        public string Dimension { get; set; }
        public string Narration { get; set; }
        public List<AccountInfoViewSubModel> Entries { get; set; }  = new List<AccountInfoViewSubModel>();
    }
    public class AccountInfoViewSubModel
    {
        private decimal? _amount;
        public string AccountName { get; set; }
        public string BranchName { get; set; }
        public decimal? Amount {
            get { return _amount; }
            set
            {
                _amount = Math.Abs(value??0);
               drcr = value < 0 ? "cr" : "dr";
            }
        }
        public string drcr { get; set; }
        public string Reference { get; set; }
    }

    public class GstModel
    {        
        public virtual decimal? GstRate { get; set; }
        public decimal? Cgst { get; set; }
        public decimal? Sgst { get; set; }
        public decimal? Igst { get; set; }
    }


    }
