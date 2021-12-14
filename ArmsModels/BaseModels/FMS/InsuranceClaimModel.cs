using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class InsuranceClaimModel
    {
        public InsuranceClaimModel()
        {
            UserInfo = new();
        }
        public int? InsuranceClaimID { get; set; }
        public int? InsuranceID { get; set; }
        public int? BreakdownID { get; set; }
        public bool IsOpen { get; set; }
        public List<string> Images { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }


    public class InsuranceClaimEventMasterModel
    {       
        public int? IcemID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }
        public bool IsMandatory { get; set; }
    }

    public class InsuranceClaimEventStatusModel
    {
        public InsuranceClaimEventStatusModel()
        {
            UserInfo = new();
        }
        public int? IcesID { get; set; }
        public int? IcemID { get; set; }
        public DateTime? EventDate { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}
