using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Model representing the type of a truck
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
        public short? TruckTypeID { get; set; } // Unique identifier for the truck type
        [Required] 
        [StringLength(maximumLength: 50)]
        public string TruckType { get; set; }
        [Required]
        public string BSType { get; set; }
        
        public byte? Axles { get; set; }
        [Required]
        public byte? wheels { get; set; }
        public byte? AdBlueFrequency { get; set; } = 0;
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}