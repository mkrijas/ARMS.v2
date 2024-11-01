using Core.BaseModels.Operations.ROI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class ROIDriverBattaModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROIDriverBattaModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public byte? Wheels { get; set; }
        [Required]
        public string BodyType { get; set; }
        [Required]
        public decimal? FromStdKM { get; set; }
        [Required]
        public decimal? ToStdKM { get; set; }
        [Required]
        public decimal? LoadingMTFrom { get; set; }
        [Required]
        public decimal? LoadingMTTo { get; set; }
        [Required]
        public decimal? DriverBatta { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}