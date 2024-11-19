using Core.BaseModels.Operations.ROI;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class ROITaxAndFCExpenseModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROITaxAndFCExpenseModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public TruckModel Truck { get; set; } = new();
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