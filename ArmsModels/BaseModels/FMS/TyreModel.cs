using ArmsModels.SharedModels;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class TyreModel
    {
        public int? TyreID { get; set; }
        public string TyreSerialNumber { get; set; }
        public string Make { get; set; } // Hint: MRF , CEAT

        public int? AssetID { get; set; }
        [Required]
        public string TyreType { get; set; } // Front/ Back/ All-Position
        [Required]
        public string TyreSize { get; set; } // 1000 x 25 etc
        public bool Tubeless { get; set; } = false;
        public UserInfoModel UserInfo { get; set; } = new();
    }
}
