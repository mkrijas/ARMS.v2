using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class GcModel
    {
        public GcModel()
        {
            UserInfo = new  SharedModels.UserInfoModel();
        }

        public long GcID { get; set; }
        public long GcSetID { get; set; }
        public long? TripID { get; set; }
        public string GcPrefix { get; set; }
        public int? GcNo { get; set; }
        public int BranchID { get; set; }
        [Required]
        public int OrderID { get; set; }
        public string OrderName { get; set; }
        [Required]
        public int RouteID { get; set; }
        public string RouteName { get; set; }
        public DateTime? GcDate { get; set; } = DateTime.Today;
        [Required]
        public int ConsignorID { get; set; }
        public string ConsignorName { get; set; }  
        [Required]
        public int ConsigneeID { get; set; }
        public string ConsigneeName { get; set; }
        [Required]
        public short? GcType { get; set; }
        public string GcTypeName { get; set; }
        [Required]
        public DateTime? BillDate { get; set; }
        public string BillNumber { get; set; }
        public decimal BillQuantity { get; set; }
        public decimal UnloadedQuantity { get; set; }
        public string PassNumber { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
