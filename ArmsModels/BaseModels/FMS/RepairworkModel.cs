using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Model representing a repair job
    public class RepairJobModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<RepairJobModel>(Json);
        }
       
        public int? RepairJobID { get; set; } // Unique identifier for the repair job
        [Required]
        public string Title { get; set; }
        public string JobCode { get; set; }
        [Required]
        public decimal? MechanicalHours { get; set; }
        public RepairJobGroup JobGroup { get; set; } // Group associated with the repair job
        public RepairJobGroup JobSubGroup { get; set; } // Subgroup associated with the repair job
        public UserInfoModel UserInfo { get; set; } = new(); // Information about the user associated with the repair job
    }

    // Model representing a group of repair jobs
    public class RepairJobGroup
    {        
        public int? ID { get; set; } // Unique identifier for the repair job group
        [Required]
        public string Title { get; set; }
        public int? ParentID { get; set; }
    }

    // Model representing a job that is currently in progress
    public class JobInProgressModel
    {
        public JobInProgressModel()
        {
            UserInfo = new();
            Mechanics = new();
        }
        public int? JipID { get; set; } // Unique identifier for the job in progress
        public int? DriverFaultID { get; set; }
        public string RepairJobTitle { get; set; }
        public int? RepairJobID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public int? JobCardID { get; set; }
        public int? WorkshopID { get; set; }
        public int? JobStatus { get; set; }
        public decimal Odometer { get; set; }
        public int? TimeTaken {  get; set; }
        public string Mechanic { get; set; }
        public decimal? TotalAmount { get; set; }

        // Property to get the status text based on JobStatus
        public string JobStatusText
        {
            get
            {
                switch (JobStatus)
                {
                    case 1: return "in Progress";
                    case 2: return "Finished";
                    case 0: return "Cancelled";
                    default: return string.Empty;
                }
            }
        }

        // Property to get the color associated with the job status
        public string JobStatusTextColor
        {
            get
            {
                switch (JobStatus)
                {
                    case 1: return "blue"; // Set the color for 'in Progress' status
                    case 2: return "green"; // Set the color for 'Finished' status
                    case 0: return "red"; // Set the color for 'Cancelled' status
                    default: return string.Empty;
                }
            }
        }
        public bool WarrantyCheck { get; set; }
        [RequiredIf("WarrantyCheck", " true")]
        public DateTime? WarrantyExpiryDate { get; set; }
        public string Remarks { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public List<MechanicJobModel> Mechanics { get; set; } // List of mechanics assigned to the job
    }

    // Model representing a mechanic's job
    public class MechanicJobModel
    {
        public MechanicJobModel()
        {
            UserInfo = new();
        }
        public int? MjID { get; set; } // Unique identifier for the mechanic job
        public int? JipID { get; set; } // Identifier for the associated job in progress
        public int? MechanicID { get; set; }
        public virtual string MechanicName { get; set; }
        public DateTime? AssignedOn { get; set; }
        public DateTime? EndedOn { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public decimal Odometer { get; set; }
    }
}