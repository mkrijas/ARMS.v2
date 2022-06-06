using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;

namespace ArmsModels.BaseModels
{
    public class CategoryModel
    {
        public CategoryModel()
        {
        }

        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class CostCenterModel
    {
        public CostCenterModel()
        {
        }

        public int? CostCenterID { get; set; }
        public string CostCenter { get; set; }
        public CategoryModel Category { get; set; } = new();
        public UserInfoModel UserInfo { get; set; } = new();
    }

    public class DimensionModel
    {

        public int? DimensionID { get; set; }
        public string Dimension { get; set; }
        public CategoryModel Category { get; set; } = new();
        public UserInfoModel UserInfo { get; set; } = new();
    }
}
