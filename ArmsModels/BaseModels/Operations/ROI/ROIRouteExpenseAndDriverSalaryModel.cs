using Core.BaseModels.Operations.ROI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class ROIRouteExpenseAndDriverSalaryModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ROIRouteExpenseAndDriverSalaryModel>(Json);
        }
        public int? ID { get; set; }
        [Required]
        public int? BranchID { get; set; } = new();
        [Required]
        public decimal? RouteExpense { get; set; }
        [Required]
        public decimal? DriverSalary { get; set; }
        [Required]
        public DateTime? FromDate { get; set; }
        [Required]
        public DateTime? ToDate { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}