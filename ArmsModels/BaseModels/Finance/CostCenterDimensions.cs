using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class CategoryModel : ICloneable
    {
        public CategoryModel()
        {
        }

        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CategoryModel>(Json);
        }
    }

    public class CostCenterModel : ICloneable
    {
        public CostCenterModel()
        {
        }

        public int? CostCenterID { get; set; }
        public string CostCenter { get; set; }
        public CategoryModel Category { get; set; } = new();
        public UserInfoModel UserInfo { get; set; } = new();

        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CostCenterModel>(Json);
        }
    }

    public class DimensionModel : ICloneable
    {

        public int? DimensionID { get; set; }
        public string Dimension { get; set; }
        public CategoryModel Category { get; set; } = new();
        public UserInfoModel UserInfo { get; set; } = new();
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DimensionModel>(Json);
        }
    }
}
