using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class UserModel
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public byte? RecordStatus { set; get; }
        public string UpdatedBy { get; set; }
        public int? LoggedBranch { get; set; }
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<UserModel>(Json);
        }
        /////////////////////////////////////////////////////////////

        public TimeSpan Validity { get; set; }
        //public Guid GuidId { get; set; }
        public DateTime ExpiredTime { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public string DeviceID { get; set; }
    }
}
