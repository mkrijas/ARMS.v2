using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TripModel
    {
        public long TripID { get; set; }
        [Required]
        public int TruckID { get; set; }        
        public string TripPrefix { get; set; }        
        public long? TripNumber { get; set; }
        [Required]
        public int BranchID { get; set; }
        [Required]
        public DateTime TripDate { get; set; }
        public decimal? Mileage { get; set; }
        public int? RunKM { get; set; }
        public decimal? Fuel { get; set; }
        public int? DriverID { get; set; }        
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
