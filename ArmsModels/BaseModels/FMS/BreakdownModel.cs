using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Model representing a breakdown incident
    public class BreakdownModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<BreakdownModel>(Json);
        }
        public BreakdownModel()
        {
            UserInfo = new();
        }
        public int? BreakdownID { get; set; } // Unique identifier for the breakdown
        public string BreakdownNumber { get; set; }
        public int? BranchID { get; set; }
        [Required]
        public string BreakdownType { get; set; }
        [Required]
        public DateTime? BreakdownTime { get; set; } = DateTime.Now;
        [Required]
        public int? TruckID { get; set; }
        public bool IsClaimInitiated { get; set; }
        public virtual string RegNo { get; set; }
        [Required]
        public string Detail { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "ContactNumber must be 10 digits long")]
        public string ContactNumber { get; set; }
        public List<EstimateListModel> EstimateList { get; set; } = new List<EstimateListModel>(); // List of estimates related to the breakdown
        public UserInfoModel UserInfo { get; set; }

        // Property to get the status of the breakdown based on the record status
        public string Status
        {
            get
            {
                switch (UserInfo.RecordStatus)
                {                   
                    case 3:
                        return "Breakdown";
                    case 4:
                        return "Estimated";
                    case 9:
                        return "Rejected";
                    default:
                        return null;
                }
            }
        }
    }

    // Model representing an estimate related to a breakdown
    public class EstimateListModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<EstimateListModel>(Json);
        }
        public int? ID { get; set; }
        public int? BreakdownID { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public string ImagePath { get; set; }
        public string ApprovedUserID { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }
}