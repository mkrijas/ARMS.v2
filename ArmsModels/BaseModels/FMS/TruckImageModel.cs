using System;
using ArmsModels.SharedModels;

namespace ArmsModels.BaseModels
{
    public class TruckImageModel
    {
        public int? ImageID { get; set; }
        public int TruckID { get; set; }
        public int? BranchID { get; set; }
        public string ImagePath { get; set; }
        public string ImageCategory { get; set; }
        public string Remarks { get; set; }
        public string BranchName { get; set; }
        public UserInfoModel UserInfo { get; set; } = new UserInfoModel();
    }
}
