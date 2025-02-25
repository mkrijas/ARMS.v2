using ArmsModels.SharedModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Represents a tariff associated with an order and route
    public class TariffModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TariffModel>(Json);
        }
        public TariffModel()
        {
            UserInfo = new UserInfoModel();
        }
        public int? TariffID { get; set; } // Unique identifier for the tariff (nullable)
        [Required]
        public OrderModel Order { get; set; } // Required property for the associated order
        [Required]
        public RouteModel Route { get; set; } // Required property for the associated route
        [Required]
        public TariffTypeModel TariffType { get; set; } // Required property for the type of tariff
        [Required]
        public TariffFormulaModel Formula { get; set; } // Required property for the formula used to calculate the tariff
        public int? TariffSign { get; set; }
        [Required]
        public decimal? TariffRate { get; set; }
        public byte? Wheels { get; set; }
        public bool CalculateOnUnloadingQty { get; set; }
        public virtual string Unit { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    // Required property for the formula used to calculate the tariff
    public class TariffFormulaModel
    {
        public TariffFormulaModel()
        {
            UserInfo = new UserInfoModel();
        }
        public short? FormulaID { get; set; } // Unique identifier for the formula (nullable)
        [Required]
        public string Formula { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    // Represents a type of tariff in the system
    public class TariffTypeModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TariffTypeModel>(Json);
        }
        public TariffTypeModel()
        {
            UserInfo = new UserInfoModel();
        }
        public short? TariffTypeID { get; set; } // Unique identifier for the tariff type (nullable)
        [Required]
        public string TariffTypeName { get; set; }
        public string TariffGroup { get; set; }
        public string Unit { get; set; }
        [Required]
        public string UsageCode { get; set; }
        public bool AllowMultiple { get; set; } = true;
        public int? TariffSign { get; set; } = 1;
        public string Area { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public bool GCCreation { get; set; }
    }
}