using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class OrderModel
    {
        public OrderModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }

        public int OrderID { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string OrderName { get; set; }
        [Required]
        public int ClientID { get; set; }        
        [Required]
        public int ContentID { get; set; } 
        public int ConsignorID { get; set; } 
        [Required]
        public decimal OrderQuantity { get; set; } 
        public ContentModel Content { get; set; }
        public PartyModel Party { get; set; }        
        public ConsigneeModel Consignor { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}

