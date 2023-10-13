using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class DriverModel
    {
        public DriverModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
            Address = new AddressModel() { AddresseeName = "Name" };
            DriverAgent = new PartyModel();
            Contacts = new();
        }
        public int? DriverID { get; set; }
        public int? DriverAgentID { get; set; }
        public PartyModel DriverAgent { get; set; }
        [Required]
        public string DriverName { get; set; }
        public int? HomeBranchID { get; set; }
        public string DriverImage { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [ValidateAge(18, ErrorMessage = "Age must be 18 years or older.")]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string AdhaarNo { get; set; }
        public string AdhaarImage { get; set; }
        public int? AddressID { get; set; }
        public string FestivalBonus { get; set; }
        public string AdditionalInfo { get; set; }
        [Required]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Mobile must be 10 digits long")]
        public string Mobile { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [ValidateComplexType]
        public AddressModel Address { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        public List<ContactModel> Contacts { get; set; }
        public bool HasValidLicense { get; set; }
        public int? TruckID { get; set; }
        public string Status
        {
            get
            {
                switch (UserInfo.RecordStatus)
                {
                    case 3:
                        return "Available";
                    case 4:
                        return "On Leave";
                    case 9:
                        return "Blacklisted";
                    default:
                        return null;
                }
            }
        }

    }

    public class DriverFaultModel
    {
        public DriverFaultModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? FaultID { get; set; }
        public DriverModel Driver { get; set; }
        public int? BranchID { get; set; }
        public DateTime? FaultDate { get; set; } = DateTime.Now;
        public byte? Severity { get; set; }
        public string Detail { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class DriverLeaveModel
    {
        public DriverLeaveModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? LeaveID { get; set; }
        public DriverModel Driver { get; set; }
        public int? BranchID { get; set; }
        public DateTime? StartTime { get; set; } = DateTime.Now;
        public DateTime? EndTime { get; set; }
        public DateTime? ExpectedReturn { get; set; }
        public string Reason { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class DriverLicenceModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DriverLicenceModel>(Json);
        }
        public DriverLicenceModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? LicenceID { get; set; }
        public int? DriverID { get; set; }
        public int? BranchID { get; set; }
        public string LicenceNo { get; set; }
        public DateTime? DLExpiryDate { get; set; }
        public string BadgeNo { get; set; }
        public DateTime? BadgeExpiryDate { get; set; }
        public string LicenceType { get; set; }
        public string DLImage { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class DriverTransferInitiationModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DriverTransferInitiationModel>(Json);
        }
        public int? DriverTransferID { get; set; }
        public DriverModel Driver { get; set; }
        public BranchModel InitiatedBranch { get; set; }
        public BranchModel DestinationBranch { get; set; }
        public DateTime? TransferInitiatedDate { get; set; } = DateTime.Now;
        public string Remarks { get; set; }
        public DriverTransferEndModel DriverTransferEndModel { get; set; }
        public int IsdriverReject { get; set; } = 0;
    }

    public class DriverTransferEndModel
    {
        public int? DriverTransferEndID { get; set; }
        public int? BranchID { get; set; }
        public bool? TransferStatus { get; set; }
        public DateTime? TransferEndDate { get; set; } = DateTime.Now;
        public string Remarks { get; set; }
        public string StatusText
        {
            get
            {
                if (TransferStatus == null)
                    return "Being Transferred";
                else if (TransferStatus.Value)
                    return "Completed";
                else
                    return "Rejected";
            }
        }
    }

    public class DriverResignModel
    {
        public DriverModel Driver { get; set; }
        [Required]
        public string Remarks { get; set; }
    }
}