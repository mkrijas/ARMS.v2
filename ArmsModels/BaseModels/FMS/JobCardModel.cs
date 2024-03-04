using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class JobcardModel
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<JobcardModel>(Json);
        }
        public JobcardModel()
        {
            UserInfo = new();
            Jobs = new();
            Workshops = new();
        }
        public int? JobcardID { get; set; }
        public string JobcardNumber { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? TruckID { get; set; }
        public virtual string RegNo { get; set; }
        public int? BranchID { get; set; }
        public string BranchName { get; set; }
        public int? BreakdownID { get; set; }
        [Required]
        public string workshop { get; set; }
        [Required]
        public string job { get; set; }       
        public string mechanic { get; set; }
        public List<JobcardWorkshopModel> Workshops { get; set; }
        public List<JobInProgressModel> Jobs { get; set; }
        public UserInfoModel UserInfo { get; set; }

        public int? PMIID { get; set; } = null;
        [Required]
        public decimal Odometer { get; set; }
    }

    //public class MaterialRequestModel
    //{
    //    public MaterialRequestModel()
    //    {
    //        UserInfo = new();
    //    }
    //    public int? MrID { get; set; }
    //    public DateTime? MrDate { get; set; }
    //    public string MrNumber { get; set; }
    //    public int? JobcardID { get; set; }
    //    public int? StoreID { get; set; }
    //    public int? BranchID { get; set; }        
    //    public UserInfoModel UserInfo { get; set; }// Status 1 for created ,2 for sent and pending, 3 for fullfilled
    //}

    public class JobcardWorkshopModel
    {
        public JobcardWorkshopModel()
        {
            UserInfo = new();
        }
        public int? JwID { get; set; }
        public int? JobCardID { get; set; }
        public int? WorkshopID { get; set; }
        public string WorkshopName { get; set; }
        public DateTime? EnteredOn { get; set; }
        public DateTime? ExitOn { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public decimal Odometer { get; set; }
    }
}