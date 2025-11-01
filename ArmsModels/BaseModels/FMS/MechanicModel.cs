using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Model representing a mechanic
    public class MechanicModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<MechanicModel>(Json);
        }
        public MechanicModel()
        {
            UserInfo = new();
        }
        public int? MechanicID { get; set; } // Unique identifier for the mechanic
        [Required]
        public string MechanicName { get; set; }
        [Required]
        public int? WorkshopID { get; set; }
        public virtual string WorkshopName { get; set; }
        public string Remarks { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    public class MechanicTransferModel
    {
        public int? TransferID { get; set; } // Unique identifier for the driver transfer initiation (nullable)   
        [Required]
        public int? MechanicID { get; set; }
        public virtual string MechanicName { get; set; }
        [Required]
        public int? CurrentWorkshopID { get; set; }
        public virtual string CurrentWorkshopName { get; set; }
        [Required]
        public int? DestinationWorkshopID { get; set; }
        public virtual string DestinationWorkshopName { get; set; }
        [Required]
        public DateTime? TransferredDate { get; set; } = DateTime.Now;
        public string Remarks { get; set; }
        public UserInfoModel UserInfo { get; set; } = new UserInfoModel();
    }
}