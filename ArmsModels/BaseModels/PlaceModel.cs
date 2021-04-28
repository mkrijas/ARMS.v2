using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class PlaceModel
    {
        public int PlaceID { get; set; }
        [Required][StringLength(maximumLength:200)]
        public string PlaceName { get; set; }
        [Required]
        public int DistrictID { get; set; }
        [Required]
        [StringLength(maximumLength: 6)]
        public string PinCode { get; set; }
        public string LatLong { get; set; }
        public long? GeoFenceID { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
