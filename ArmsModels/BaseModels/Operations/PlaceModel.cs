using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Represents a place in the system
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
        public int? PlaceID { get; set; } // Unique identifier for the place (nullable)
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

    // Represents a state in the system
    public class StateModel
    {
        public int? StateID { get; set; } // Unique identifier for the state (nullable)
        [Required]
        [StringLength(maximumLength: 200)]
        public string StateName { get; set; }
        public int? GstCode { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Represents a district in the system
    public class DistrictModel
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DistrictModel>(Json);
        }
        public int? DistrictID { get; set; } // Unique identifier for the district (nullable)
        [Required]
        [StringLength(maximumLength: 200)]
        public string DistrictName { get; set; }
        [Required]
        public int? StateID { get; set; }
        public virtual StateModel State { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}