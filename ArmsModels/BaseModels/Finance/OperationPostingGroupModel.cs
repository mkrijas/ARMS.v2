using ArmsModels.SharedModels;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class OperationPostingGroupModel
    {
        public int? Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public GstUsageCodeModel UsageCode { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }
}