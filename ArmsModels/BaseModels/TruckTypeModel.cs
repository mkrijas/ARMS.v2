using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TruckTypeModel
    {
        public short TruckTypeID { get; set; }
        [Required][StringLength(maximumLength:50)]
        public string TruckType { get; set; }
        [Required]
        public decimal UnladenWeight { get; set; }
        [Required]
        public decimal GrossWeight { get; set; }
        [Required]
        public byte Axles { get; set; }
        [Required]
        public byte wheels { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }      
    }
}
