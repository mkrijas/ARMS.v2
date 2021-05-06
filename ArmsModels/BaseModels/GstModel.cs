using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class GstModel
    {
        public int GstID { get; set; }
        [Required]
        [StringLength(maximumLength: 15)]
        public string GstNo { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string RegName { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string TradeName { get; set; }
        [Required]
        [StringLength(maximumLength: 10)]
        public string TanNo { get; set; }
        public int PartyID { get; set; }
        public int AddressID { get; set; }
        [Required]
        [StringLength(maximumLength: 10)]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
