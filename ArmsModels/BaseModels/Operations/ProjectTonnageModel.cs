using ArmsModels.BaseModels;
using ArmsModels.SharedModels;

namespace Core.BaseModels.Operations
{
    public class ProjectTonnageModel
    {
        public int? ID { get; set; }
        public int? BranchID { get; set; }
        public string BranchName { get; set; }
        public string BodyType { get; set; }
        public int? Wheels { get; set; }
        public decimal? Tonnage { get; set; }
        public int? CountTrips { get; set; }
        public decimal? Freight { get; set; }
        public int? ProjectedTrips { get; set; }
        public decimal? ProjectedTonnage { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}