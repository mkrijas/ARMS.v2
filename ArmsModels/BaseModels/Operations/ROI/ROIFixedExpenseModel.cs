using Core.BaseModels.Operations.ROI;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class ROIFixedExpenseModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROIFixedExpenseModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public byte? Wheels { get; set; }
        [Required]
        public ROITonnageModel BSType { get; set; } = new();
        [Required]
        public string BodyType { get; set; }
        [Required]
        public decimal? BranchAdmin { get; set; }
        [Required]
        public decimal? HOAdmin { get; set; }
        [Required]
        public decimal? Tax { get; set; }
        [Required]
        public decimal? Maintenance { get; set; }
        [Required]
        public decimal? Tyre { get; set; }
        [Required]
        public decimal? TaxAndInsurance { get; set; }
        [Required]
        public decimal? FC { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}