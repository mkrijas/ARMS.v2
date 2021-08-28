using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TruckModel
    {
        public TruckModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
            CurrentRegistration = new TruckRegistrationModel();
        }

        public int? TruckID { get; set; }
        public string RegNo { get; set; }
        public int? HomeBranchID { get; set; }
        [Required]
        public short? TruckTypeID { get; set; }
        public string TruckType { get; set; }
        [Required][StringLength(maximumLength:50)]
        public string BodyType { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string ChassisNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string EngineNumber { get; set; }
        public short? ManufacturedYear { get; set; }
        public long? GpsDeviceID { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string TransmissionType { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string FuelType { get; set; }
        [Required]
        public decimal? FuelTankCapacity { get; set; }
        [Required]
        public DateTime? PurchaseDate { get; set; }
        public DateTime? SoldDate { get; set; }
        public TruckRegistrationModel CurrentRegistration { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }

   
    }

    public class TruckDataArrayModel
    {
        private readonly IConfiguration Configuration;

        public TruckDataArrayModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public const string Data = "Data";

        //public string[] BodyTypes { get; set; }
        //public string[] FuelTypes { get; set; }
        //public string[] TransmissionTypes { get; set; }

        public string[] BodyTypes { get { return Configuration.GetSection(Data).GetSection("BodyTypes").Get<string[]>(); } }
        public string[] FuelTypes { get { return Configuration.GetSection(Data).GetSection("FuelTypes").Get<string[]>(); } }
        public string[] TransmissionTypes { get { return Configuration.GetSection(Data).GetSection("TransmissionTypes").Get<string[]>(); } }

    }
}
