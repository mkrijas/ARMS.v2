using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class GstUsageIDModel
    {
        public GstUsageIDModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }

        [Required]
        [StringLength(maximumLength: 25)]
        public string UsageID { get; set; }
        
        public int? AccountID { get; set; }
        public virtual string AccountName { get; set; }
        [Required]
        public int? RID { get; set; }
        public virtual decimal? TaxRate { get; set; }

        [StringLength(maximumLength: 6)]
        public string SAC { get; set; }
        [Required]
        public DateTime? PeriodFrom { get; set; }
        [Required]
        public DateTime? PeriodTo { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }

    }


    public class GstRateModel
    {
        public int? RID { get; set; }
        public decimal? TaxRate { get; set; }
        public string Description { get; set; }
    }

    public class GstItemModel
    {
        public GstItemModel(string _itemCode,decimal? _taxRate)
        {
            ItemCode = _itemCode;
            TaxRate = _taxRate;
            UserInfo = new SharedModels.UserInfoModel();
        }
        public GstItemModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? HsnID { get; set; }
        [Required]
        public int? ItemID { get; set; }
        public virtual string ItemCode { get; }
        [Required]
        public int? RID { get; set; }

        public virtual decimal? TaxRate { get; }        
        [Required]
        public DateTime? PeriodFrom { get; set; }
        [Required]
        public DateTime? PeriodTo { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
   
}
