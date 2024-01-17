using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TruckTypeModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TruckTypeModel>(Json);
        }
        public TruckTypeModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public short? TruckTypeID { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string TruckType { get; set; }
        [Required]
        public string BSType { get; set; }
        
        public byte? Axles { get; set; }
        [Required]
        public byte? wheels { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}