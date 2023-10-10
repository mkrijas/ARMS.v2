using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
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
        public int? MechanicID { get; set; }
        public string MechanicName { get; set; }
        public int? WorkshopID { get; set; }
        public virtual string WorkshopName { get; set; }
        public string Remarks { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}