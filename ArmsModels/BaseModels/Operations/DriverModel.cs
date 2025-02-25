using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Represents a driver in the system
    public class DriverModel
    {
        public DriverModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
            Address = new AddressModel() { AddresseeName = "Name" };
            DriverAgent = new PartyModel();
            Contacts = new();
        }
        public int? DriverID { get; set; } // Unique identifier for the driver (nullable)
        public string DriverCode { get; set; }
        public int? DriverAgentID { get; set; }
        public PartyModel DriverAgent { get; set; } // Driver agent information
        [Required]
        public string DriverName { get; set; }
        public DateTime? JoiningDate { get; set; }
        public int? HomeBranchID { get; set; }
        public string HomeBranchName { get; set; }
        public string DriverImage { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [ValidateAge(21, ErrorMessage = "Age must be 21 years or older.")]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string AdhaarNo { get; set; }
        public string AdhaarImage { get; set; }
        public int? AddressID { get; set; }
        public string FestivalBonus { get; set; }
        public string AdditionalInfo { get; set; }
        public int? BankAccountID { get; set; }
        [Required]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Mobile must be 10 digits long")]
        public string Mobile { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [ValidateComplexType]
        public AddressModel Address { get; set; } // Address information for the driver
        public BankAccountModel BankAccount { get; set; } // Bank account information for the driver (optional)
        public DriverLicenceModel DriverLicence { get; set; } = new(); // Driver's license information
        public SharedModels.UserInfoModel UserInfo { get; set; } = new(); // List of contacts associated with the driver
        public List<ContactModel> Contacts { get; set; }
        public bool HasValidLicense { get; set; }
        public int? TruckID { get; set; }
        public virtual DateTime? SinceLastEvent { get; set; }

        // Property to get the driver's status based on UserInfo.RecordStatus
        public string Status
        {
            get
            {
                switch (UserInfo.RecordStatus)
                {
                    case 0:
                        return "Resigned";
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

    // Represents a fault associated with a driver
    public class DriverFaultModel
    {
        public DriverFaultModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        } 
        public int? FaultID { get; set; }  // Unique identifier for the fault (nullable)
        public DriverModel Driver { get; set; }
        public int? BranchID { get; set; }
        [Required]
        public DateTime? FaultDate { get; set; } = DateTime.Now;
        [Required]
        public byte? Severity { get; set; }
        [Required] 
        public decimal? Amount { get; set; }
        [Required]
        public string Detail { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Represents a leave request associated with a driver
    public class DriverLeaveModel
    {
        public DriverLeaveModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? LeaveID { get; set; } // Unique identifier for the leave request (nullable)
        public DriverModel Driver { get; set; }
        public int? BranchID { get; set; }
        public DateTime? StartTime { get; set; } = DateTime.Now;
        public DateTime? EndTime { get; set; }
        public DateTime? ExpectedReturn { get; set; }
        public string Reason { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Represents a driver's license information
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
        public int? LicenceID { get; set; } // Unique identifier for the driver's license (nullable)
        public int? DriverID { get; set; }
        public int? BranchID { get; set; }
        [StringLength(16, MinimumLength = 15, ErrorMessage = "Licence number must be between 15 and 16 characters.")]
        public string LicenceNo { get; set; }
        public DateTime? DLExpiryDate { get; set; }
        public string BadgeNo { get; set; }
        public DateTime? BadgeExpiryDate { get; set; }
        public string LicenceType { get; set; }
        public string DLImage { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }

    // Represents the initiation of a driver transfer
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

    public class DriverWorkPeriodModel
    {
        public DriverModel Driver { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}