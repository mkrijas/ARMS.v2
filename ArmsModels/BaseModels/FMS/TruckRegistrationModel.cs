using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TruckRegistrationModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TruckRegistrationModel>(Json);
        }
        public int? RegID { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string RegNo { get; set; }
        public int? TruckID { get; set; }
        [Required]
        public DateTime? EffectFrom { get; set; }
        [Required]
        public DateTime? EffectTo { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string RC { get; set; }  //Url of RC doc
        public bool IsValid { get { return DateTime.Today <= EffectTo && DateTime.Today >= EffectFrom; } }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}