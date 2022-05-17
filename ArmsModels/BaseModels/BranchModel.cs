using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class BranchModel
    {
        public BranchModel()
        {
            Address = new AddressModel();
            UserInfo = new SharedModels.UserInfoModel();
            Place = new PlaceModel();
            Contacts = new();
        }
        public int? BranchID { get; set; }
        [Required]
        [StringLength(256,MinimumLength =3)]
        public string BranchName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Gst number must have 15 characters")]
        public string GstNo { get; set; }
        public int? PlaceID { get; set; }
        public int? AddressID { get; set; }
        public bool Operate { get; set; } = true;
        public int? UpwardBranchID { get; set; } = 0;
        public bool Active { get; set; } = true;
        [ValidateComplexType]
        public virtual AddressModel Address { get; set; }        
        public PlaceModel Place { get; set; }
        public virtual SharedModels.UserInfoModel UserInfo { get; set; }
        public List<ContactModel> Contacts { get; set; }
    }
}
