using ArmsModels.BaseModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Finance
{
    // Model representing a request to open a financial day
    public class DayOpenRequestModel
    {
        public int? ID { get; set; } // Unique identifier for the day open request
        public BranchModel Branch { get; set; } = new(); // Information about the branch associated with the request
        [Required]
        public DocTypeModel DocType { get; set; } // Document type associated with the request
        [Required] 
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        public bool? IsOpen { get; set; }
        public DateTime? OpenDate { get; set; }
        public string OpenByUsedID { get; set; }
        public DateTime? CloseDate { get; set; }
        public string CloseByUsedID { get; set; }
        public byte? RecordStatus { get; set; }
        public ArmsModels.SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}