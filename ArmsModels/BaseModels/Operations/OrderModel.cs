using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Represents an order in the system
    public class OrderModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<OrderModel>(Json);
        }
        public OrderModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public int? OrderID { get; set; } // Unique identifier for the order (nullable)
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
        public decimal? OrderQuantity { get; set; }
        public ContentModel Content { get; set; } = new(); // Content information associated with the order
        public PartyModel Party { get; set; } // Party information associated with the order (optional)
        public ConsigneeModel Consignor { get; set; } // Consignor information associated with the order (optional)
        public SharedModels.UserInfoModel UserInfo { get; set; }
        public string Declaration { get; set; }
    }
}