using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    // Model representing a director associated with a party
    public class PartyDirectorModel
    {
        public PartyDirectorModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }

        public int? PartyDirectorID { get; set; } // Unique identifier for the party director
        [Required]
        public int? PartyID { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string PersonName { get; set; }
        [Required]
        [StringLength(maximumLength: 10)]
        public string Phone { get; set; }
        [Required]
        [StringLength(maximumLength: 10)]
        public string Pan { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
