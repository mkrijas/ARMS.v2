using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TruckModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TruckModel>(Json);
        }
        public TruckModel()
        {
            CurrentRegistration = new TruckRegistrationModel();
            UserInfo = new SharedModels.UserInfoModel();
            CurrentEvent = new();
        }
        public int? TruckID { get; set; }
        public string RegNo { get; set; }
        public int? HomeBranchID { get; set; }
        public string HomeBranchName { get; set; }
        public string OperatingBranchName { get; set; }
        public int? CurrentBranchID { get; set; }
        [Required]
        public short? TruckTypeID { get; set; }
        public string TruckType { get; set; }
        public string BSType { get; set; }
        public byte? wheels { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
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
        public decimal? SecondFuelTankCapacity { get; set; }
        [Required]
        public decimal? UnladenWeight { get; set; }
        [Required]
        public decimal? GrossWeight { get; set; }
        [Required]
        public DateTime? PurchaseDate { get; set; }
        public DateTime? SoldDate { get; set; }
        public EventModel CurrentEvent { get; set; }
        [Required]
        [ValidateComplexType]
        public TruckRegistrationModel CurrentRegistration { get; set; }
        [Required]
        public int? AssetID { get; set; }
        SharedModels.UserInfoModel _userInfo;
        public SharedModels.UserInfoModel UserInfo
        {
            get { return _userInfo; }
            set
            {
                _userInfo = value;
                CurrentRegistration.UserInfo = _userInfo;
            }
        }
    }

    public class TruckDataArrayModel
    {
        private readonly IConfiguration Configuration;
        public TruckDataArrayModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public const string Data = "Data";
        public string[] BodyTypes { get { return Configuration.GetSection(Data).GetSection("BodyTypes").Get<string[]>(); } }
        public string[] FuelTypes { get { return Configuration.GetSection(Data).GetSection("FuelTypes").Get<string[]>(); } }
        public string[] TransmissionTypes { get { return Configuration.GetSection(Data).GetSection("TransmissionTypes").Get<string[]>(); } }
        public string[] BankTools { get { return Configuration.GetSection(Data).GetSection("BankTools").Get<string[]>(); } }
        public string[] NatureOfTransaction { get { return Configuration.GetSection(Data).GetSection("NatureOfTransaction").Get<string[]>(); } }
    }

    public class TruckStatusModel
    {
        public string Truck { get; set; }
        public int? DisplayOrder { get; set; }
        public string StatusText { get; set; }
        public int? NoOfTrucks { get; set; }
        public string LoadStatus { get; set; }
        public virtual int? TruckID { get; set; }
        public virtual string RegNo { get; set; }
        public virtual DateTime? EventTime { get; set; }
        public virtual string? EventDateTimeDiff { get; set; }
        public string DriverSince { get; set; }
    }
}