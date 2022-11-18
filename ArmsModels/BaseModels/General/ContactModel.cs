using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;

namespace ArmsModels.BaseModels
{
    public class ContactModel
    {
        public ContactModel()
        {
            UserInfo = new UserInfoModel();
        }

        public int? ContactID { get; set; }
        [Required]
        public string ContactName { get; set; }        
        public string AdditionalInfo { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Mobile must be 10 digits long")]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }       
        public string RefTable { get; set; }
        public int? RefKey { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}
