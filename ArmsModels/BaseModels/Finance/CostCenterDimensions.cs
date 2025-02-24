using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Model representing a category
    public class CategoryModel : ICloneable
    {
        public CategoryModel()
        {

        }
        public int? CategoryID { get; set; } // Unique identifier for the category
        public string CategoryName { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CategoryModel>(Json);
        }
    }

    // Model representing a cost center
    public class CostCenterModel : ICloneable
    {
        public CostCenterModel()
        {

        }
        public int? CostCenterID { get; set; } // Unique identifier for the cost center
        public string CostCenter { get; set; }
        public CategoryModel Category { get; set; } = new(); // Associated category for the cost center
        public UserInfoModel UserInfo { get; set; } = new();
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CostCenterModel>(Json);
        }
    }

    // Model representing a dimension
    public class DimensionModel : ICloneable
    {
        public int? DimensionID { get; set; } // Unique identifier for the dimension
        public string Dimension { get; set; }
        public CategoryModel Category { get; set; } = new(); // Associated category for the dimension
        public UserInfoModel UserInfo { get; set; } = new();
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DimensionModel>(Json);
        }
    }
}