using Core.BaseModels.Operations.ROI;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class ROITyreAndMaintenanceExpenseModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROITyreAndMaintenanceExpenseModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public TruckTypeModel TruckType { get; set; } = new();
        [Required]
        public decimal? Maintenance { get; set; }
        [Required]
        public decimal? Tyre { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}