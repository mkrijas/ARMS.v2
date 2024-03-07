using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class WorkshopModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<WorkshopModel>(Json);
        }
        public WorkshopModel()
        {
            UserInfo = new();
        }
        public int? WorkshopID { get; set; }
        [Required]
        public string WorkshopName { get; set; }
        [Required]
        public string WorkshopType { get; set; } // Inhouse/Outside
        [Required]
        [StringLength(10, ErrorMessage = "Contact number should digit", MinimumLength = 10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Contact Number must be numeric")]
        public string ContactNumber { get; set; }
        [RequiredIf("WorkshopType","Outside",ErrorMessage = "Outside workshop cannot be created without party!")]
        public int? PartyID { get; set; }
        public PartyModel Party { get; set; }
        public int? GstID { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}