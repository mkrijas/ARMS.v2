using System;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class ContactModel : ICloneable
    {
        // Model representing a contact
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ContactModel>(Json);
        }
        public ContactModel()
        {
            UserInfo = new UserInfoModel();
        }
        public int? ContactID { get; set; } // Unique identifier for the contact
        [Required]
        public string ContactName { get; set; }
        public string AdditionalInfo { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Mobile must be 10 digits long")]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string RefTable { get; set; }
        public int? RefKey { get; set; }
        public int? Index { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}