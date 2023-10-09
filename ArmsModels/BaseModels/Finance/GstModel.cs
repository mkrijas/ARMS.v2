using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class GstUsageCodeModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<GstUsageCodeModel>(Json);
        }
        public GstUsageCodeModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? Id { get; set; }
        public virtual string UsageCode { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        public string Description { get; set; }
        [Required(ErrorMessage = "The Account field is required!")]
        public int? CoaID { get; set; }
        public virtual string CoaDescreption { get; set; }
        [Required]
        public string Area { get; set; } = "All"; // Operation,Maintenance,All
        [Required]
        public int? RID { get; set; }
        public virtual decimal? TaxRate { get; set; }
        [StringLength(maximumLength: 8)]
        [Required]
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

    public class GstItemModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<GstItemModel>(Json);
        }
        public GstItemModel(string _itemCode, decimal? _taxRate)
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
        public string ItemDescription { get; set; }
        public string HsnCode { get; set; }
        [Required]
        public int? RID { get; set; }
        public virtual decimal? TaxRate { get; }
        [Required]
        public DateTime? PeriodFrom { get; set; }
        [Required]
        public DateTime? PeriodTo { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class GstInOutModel
    {
        public int? GstTypeID { get; set; }
        public string GstType { get; set; }
        public string InputAccount { get; set; }
        public string OutputAccount { get; set; }
    }
}