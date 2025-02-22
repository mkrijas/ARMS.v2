using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    // Model representing a GST usage code
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
        public int? Id { get; set; } // Unique identifier for the GST usage code
        public virtual string UsageCode { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Description { get; set; }
        [Required(ErrorMessage = "The Account field is required!")]
        public int? CoaID { get; set; }
        public virtual string CoaDescreption { get; set; }
        public virtual string CoaAccountCode { get; set; }
        [Required]
        public string GstMechanism { get; set; } = "FCM"; // Operation,Maintenance,All
        [Required]
        public int? RID { get; set; }
        public virtual decimal? TaxRate { get; set; }
        [StringLength(maximumLength: 8)]        
        public string SAC { get; set; }
        [Required]
        public DateTime? PeriodFrom { get; set; }
        [Required]
        public DateTime? PeriodTo { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Model representing a GST rate
    public class GstRateModel
    {
        public int? RID { get; set; }
        public decimal? TaxRate { get; set; }
        public string Description { get; set; }
    }

    // Model representing a GST item
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
        public int? HsnID { get; set; } // Unique identifier for the HSN code
        [Required]
        public int? ItemID { get; set; }
        public string GstMechanism { get; set; } = "FCM"; // Operation,Maintenance,All
        public virtual string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string HsnCode { get; set; }
        [Required]
        public int? RID { get; set; }
        public virtual string ItemGroupDescription { get; set; }
        public virtual decimal? TaxRate { get; set; }
        [Required]
        public DateTime? PeriodFrom { get; set; }
        [Required]
        public DateTime? PeriodTo { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Model representing GST input and output accounts
    public class GstInOutModel
    {
        public int? GstTypeID { get; set; }
        public string GstType { get; set; }
        public string InputAccount { get; set; }
        public string OutputAccount { get; set; }
    }
}