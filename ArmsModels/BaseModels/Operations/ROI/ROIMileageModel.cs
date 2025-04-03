using Core.BaseModels.Operations.ROI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class ROIMileageModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROIMileageModel>(Json);
        }
        public int? ID { get; set; }
        //public TruckTypeModel TruckType { get; set; }
        public short? TruckTypeID { get; set; }
        public virtual string TruckType { get; set; }
        public virtual string BSType { get; set; }
        [Required]
        public string BodyType { get; set; }
        [Required]
        public decimal? LoadingMTFrom { get; set; }
        [Required]
        public decimal? LoadingMTTo { get; set; }
        [Required]
        public decimal? Mileage { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}