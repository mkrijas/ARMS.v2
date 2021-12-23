using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class RepairJobModel
    {
        public RepairJobModel()
        {
            UserInfo = new();
        }

        public int? RepairJobID { get; set; }
        [Required]
        public string RepairJobTitle { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal? MechanicalHours { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }


    public class JobInProgressModel
    {
        public JobInProgressModel()
        {
            UserInfo = new();
            Mechanics = new();
        }
        public int? JipID { get; set; }
        public string RepairJobTitle { get; set; }
        public int? RepairJobID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public int? JobCardID { get; set; }
        public int? WorkshopID { get; set; }
        public int? JobStatus { get; set; }
        public string Remarks { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public List<MechanicJobModel> Mechanics { get; set; }
    }

    public class MechanicJobModel
    {
        public MechanicJobModel()
        {
            UserInfo = new();
        }
        public int? MjID { get; set; }
        public int? JipID { get; set; }
        public int? MechanicID { get; set; }
        public virtual string MechanicName { get; set; }
        public DateTime? AssignedOn { get; set; }
        public DateTime? EndedOn { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}
