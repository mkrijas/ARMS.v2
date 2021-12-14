using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class RouteModel
    {
        public RouteModel()
        {
            this.Origin = new PlaceModel();
            this.Via = new PlaceModel();
            this.Destination = new PlaceModel();
            this.UserInfo = new SharedModels.UserInfoModel();
        }

        public int? RouteID { get; set; }        
        public string RouteName { get; set; }        
        public PlaceModel Origin { get; set; }        
        public PlaceModel Via { get; set; }        
        public PlaceModel Destination { get; set; }
        
        public long? GpsRouteID { get; set; }
        [Required]
        public decimal? Distance { get; set; }
        public byte? SpeedLimit { get; set; }
        [Required]
        public short? RunningHours { get; set; }
        public decimal? MieageModifier { get; set; }
        [Required]
        public string RouteType { get; set; }
        [Required]
        public byte? TollBooths { get; set; }        
        public SharedModels.UserInfoModel UserInfo { get; set; }

    }
}
