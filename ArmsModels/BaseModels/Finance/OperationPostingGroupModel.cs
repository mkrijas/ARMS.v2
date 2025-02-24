using ArmsModels.SharedModels;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    // Model representing an operation posting group
    public class OperationPostingGroupModel
    {
        public int? ID { get; set; }
        public string ADCode { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public GstUsageCodeModel UsageCode { get; set; } // Associated GST usage code for the operation posting group
        public UserInfoModel UserInfo { get; set; } = new();
    }
}