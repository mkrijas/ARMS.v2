using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class RouteModel
    {
        public int RouteID { get; set; }
        [Required]
        public string RouteName { get; set; }
        [Required]
        public int Origin { get; set; }
        [Required]
        public int Via { get; set; }
        [Required]
        public int Destination { get; set; }
        
        public long? GpsRouteID { get; set; }
        [Required]
        public decimal Distance { get; set; }
        public byte? SpeedLimit { get; set; }
        [Required]
        public short RunningHours { get; set; }
        public decimal? MieageModifier { get; set; }
        [Required]
        public string RouteType { get; set; }
        [Required]
        public byte TollBooths { get; set; }        
        public SharedModels.UserInfoModel UserInfo { get; set; }

    }
}
