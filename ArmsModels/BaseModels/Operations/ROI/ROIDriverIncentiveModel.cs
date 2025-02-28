using Core.BaseModels.Operations.ROI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class ROIDriverIncentiveModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROIDriverIncentiveModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public byte? Wheels { get; set; }
        [Required]
        public string BSType { get; set; }
        public OrderModel Order { get; set; } = new();
        public StateModel State { get; set; } = new();
        [Required]
        public decimal? LoadingMTFrom { get; set; }
        [Required]
        public decimal? LoadingMTTo { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}