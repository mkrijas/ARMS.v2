using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class PlaceModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<PlaceModel>(Json);
        }
        public PlaceModel()
        {
            this.District = new DistrictModel();
            this.UserInfo = new SharedModels.UserInfoModel();
        }
        public int? PlaceID { get; set; }
        public int? GstCode { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string PlaceName { get; set; }
        public int? DistrictID { get; set; }
        [Required]
        [StringLength(maximumLength: 6)]
        public string PinCode { get; set; }
        public dynamic LatLong { get; set; }
        public long? GeoFenceID { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        public virtual DistrictModel District { get; set; }
    }

    public class StateModel
    {
        public int? StateID { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string StateName { get; set; }
        public string GstString { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class DistrictModel
    {
        public int? DistrictID { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string DistrictName { get; set; }
        [Required]
        public int? StateID { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}