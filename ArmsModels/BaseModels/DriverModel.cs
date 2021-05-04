using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class DriverModel
    {
        public int DriverID { get; set; }
        [Required]
        public string DriverName { get; set; }
        [Required]
        public string DriverImage { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string AdhaarNo { get; set; }
        [Required]
        public string AdhaarImage { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string AlternateContactPerson { get; set; }
        public string AlternateContactMobile { get; set; }
        public int AddressID { get; set; }
        public int DriverAgentID { get; set; }
        public string FestivalBonus { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
    public class DriverBranchPeriods
    {
      public int  DriverPeriodID { get; set; }
        [Required]
        public int DriverID { get; set; }
        [Required]
        public short BranchID { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public bool Joining { get; set; }
        [Required]
        public bool Resignation { get; set; }
        public string Remarks { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }

    }
    public class DriverFault
    {
      public int FaultID { get; set; }
        public int DriverID { get; set; }
        public short BranchID { get; set; }
        public DateTime FaultDate { get; set; }
        public byte Severity { get; set; }
        public string Detail { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
    public class DriverLeaves
    {
        public int LeaveID { get; set; }
        public int DriverID { get; set; }
        public short BranchID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ExpectedReturn { get; set; }
        public string Reason { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
    public class DriverLicence
    {
        public int LicenceID { get; set; }
        public int DriverID { get; set; }
        public short BranchID { get; set; }
        public string LicenceNo { get; set; }
        public DateTime DLExpiryDate { get; set; }
        public string BadgeNo { get; set; }
        public DateTime BadgeExpiryDate { get; set; }
        public string LicenceType { get; set; }
        public string DLImage { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

}
