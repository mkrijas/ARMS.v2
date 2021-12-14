using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class WorkshopModel
    {
        public WorkshopModel()
        {
            UserInfo = new();
        }

        public int? WorkshopID { get; set; }
        //[Required]
        public string WorkshopName { get; set; }
       // [Required]
        public string WorkshopType { get; set; } // Inhouse/Outside
        //[Required]
        public string ContactNumber { get; set; }
        public int? PartyID { get; set; }
        public int? GstID { get; set; }       
        public UserInfoModel UserInfo { get; set; }
    }
   
}
