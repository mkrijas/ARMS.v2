using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string OrderName { get; set; }
        [Required]
        public int ClientID { get; set; }
        [Required]
        public int RouteID { get; set; }
        [Required]
        public int ContentID { get; set; }
        [Required]
        public int ConsignorID { get; set; }
        [Required]
        public int ConsigneeID { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}

