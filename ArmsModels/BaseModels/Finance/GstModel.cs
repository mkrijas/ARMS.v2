using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class GstUsageCodeModel
    {
        public GstUsageCodeModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }        
        public int? Id { get; set; }        
        public virtual string UsageCode { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        public string Description { get; set; }
        public int? CoaID { get; set; }
        public virtual string CoaDescreption { get; set; }
        [Required]
        public string Area { get; set; } // Operation,Maintenance,All
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
