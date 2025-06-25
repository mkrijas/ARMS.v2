using ArmsModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Model representing an address
    public class AddressModel
    {
        public AddressModel()
        {
            UserInfo = new UserInfoModel();
        }

        public int? AddressID { get; set; } // Unique identifier for the address
        [Required]
        [StringLength(maximumLength: 200,MinimumLength =3)]
        public string AddresseeName { get; set; }

        [StringLength(maximumLength: 200, MinimumLength = 3)]
        public string Building { get; set; }

        [StringLength(maximumLength: 200, MinimumLength = 3)]
        public string Street { get; set; }
        [Required]
        [StringLength(maximumLength: 200, MinimumLength = 3)]
        public string Place { get; set; }
        [Required]
        [StringLength(maximumLength: 200, MinimumLength = 3)]
        public string City { get; set; }
        [Required]
        [StringLength(maximumLength: 6,MinimumLength =6)]
        public string PinCode { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
        public virtual bool IsDefault { get; set; }
        public int? PartyID { get; set; } // Unique identifier for the address

    }
}
