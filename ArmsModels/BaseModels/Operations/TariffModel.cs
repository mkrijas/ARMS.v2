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
        public int? TariffID { get; set; }
        [Required]
        public OrderModel Order { get; set; }
        [Required]
        public RouteModel Route { get; set; }
        [Required]
        public TariffTypeModel TariffType { get; set; }
        [Required]
        public TariffFormulaModel Formula { get; set; }
        public int? TariffSign { get; set; }
        [Required]
        public decimal? TariffRate { get; set; }
        public byte? Wheels { get; set; }
        public bool CalculateOnUnloadingQty { get; set; }
        public virtual string Unit { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    public class TariffFormulaModel
    {
        public TariffFormulaModel()
        {
            UserInfo = new UserInfoModel();
        }
        public short? FormulaID { get; set; }
        [Required]
        public string Formula { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

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
        public short? TariffTypeID { get; set; }
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
    }
}