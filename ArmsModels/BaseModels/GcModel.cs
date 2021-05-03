using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class GcModel
    {
        public long GcID { get; set; }
        public long? TripID { get; set; }
        public string GcPrefix { get; set; }
        public int? GcNo { get; set; }
        public int BranchID { get; set; }
        public int OrderID { get; set; }
        public DateTime GcDate { get; set; }
        public int ConsignorID { get; set; }
        public int ConsigneeID { get; set; }
        public short GcType { get; set; }
        public DateTime BillDate { get; set; }
        public string BillNumber { get; set; }
        public decimal BillQuantity { get; set; }
        public decimal UnloadedQuantity { get; set; }
        public string PassNumber { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
