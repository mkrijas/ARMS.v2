using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class InsuranceClaimModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InsuranceClaimModel>(Json);
        }
        public InsuranceClaimModel()
        {
            UserInfo = new();
            Images = new();
        }
        public int? InsuranceClaimID { get; set; }
        public int? InsuranceID { get; set; }
        public int? BreakdownID { get; set; }
        public bool IsOpen { get; set; }
        [Required]
        public string ClaimNo { get; set; }
        public string TruckRegNo { get; set; }
        [Required]
        public string PolicyNo { get; set; }
        [Required]
        public DateTime? DateOfAccident { get; set; }
        public decimal? EstimateAmount { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public List<string> Images { get; set; }
        public string RemoveImage { get; set; }
        [Required]
        public string Notes { get; set; }
        public virtual AssetModel Asset { get; set; }
        public virtual List<InsuranceClaimEventStatusModel> Events { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public virtual string BranchName { get; set; }
    }


    public class InsuranceClaimEventMasterModel
    {
        public InsuranceClaimEventMasterModel()
        {
            UserInfo = new();
        }
        public int? IcemID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }
        public bool IsMandatory { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    public class InsuranceClaimEventStatusModel
    {
        public InsuranceClaimEventStatusModel()
        {
            UserInfo = new();
        }
        public int? IcesID { get; set; }
        [Required]
        public int? IcemID { get; set; }
        public virtual string Title { get; set; }
        [Required]
        public int? InsuranceClaimID { get; set; }
        [Required]
        public DateTime? EventDate { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}
