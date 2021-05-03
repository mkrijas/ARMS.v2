using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TruckModel
    {
        public int TruckID { get; set; }
        public string RegNumber { get; set; }
        [Required]
        public short TruckTypeID { get; set; }
        [Required][StringLength(maximumLength:50)]
        public string BodyType { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string ChassisNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string EngineNumber { get; set; }
        public short ManufacturedYear { get; set; }
        public long? GpsDeviceID { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string TransmissionType { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string FuelType { get; set; }
        [Required]
        public decimal FuelTankCapacity { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        public DateTime? SoldDate { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }

   
    }
}
