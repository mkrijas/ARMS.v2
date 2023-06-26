using Newtonsoft.Json.Serialization;
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

        public int? OrderID { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string OrderName { get; set; }        
        public int? ClientID { get; set; }        
        [Required]
        public short? ContentID { get; set; } 
        public string? GstNo { get; set; }
        public int? ConsignorID { get; set; }
        public bool IsLimitedQuantity { get; set; }
        [RequiredIfTrue("IsLimitedQuantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Specify the Quantity")]
        public decimal? OrderQuantity { get; set; } 
        public ContentModel Content { get; set; }
        public PartyModel Party { get; set; }        
        public ConsigneeModel Consignor { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}

