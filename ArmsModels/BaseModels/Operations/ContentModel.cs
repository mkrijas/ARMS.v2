using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class ContentModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ContentModel>(Json);
        }
        public ContentModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public short? ContentID { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string ContentName { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string PrimaryUnit { get; set; }
        [StringLength(maximumLength: 100)]
        public string SecondaryUnit { get; set; }
        public decimal? UnitRatio { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}