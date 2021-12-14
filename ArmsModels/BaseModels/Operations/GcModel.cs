using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{

    public class GcSetModel
    {
        public GcSetModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
            Gcs = new();
        }        
        public long? GcSetID { get; set; }
        public long? TripID { get; set; }        
        public int? BranchID { get; set; }
        [Required]
        public int? OrderID { get; set; }
        public virtual string OrderName { get; set; }
        [Required]
        public int? RouteID { get; set; }
        public virtual string RouteName { get; set; }
        public DateTime? GcDate { get; set; } = DateTime.Today;
        [Required]
        public int? ConsignorID { get; set; }
        public virtual string ConsignorName { get; set; }
        [Required]
        public int? ConsigneeID { get; set; }
        public byte? PaidBy { get; set; }
        public virtual string ConsigneeName { get; set; }

        public virtual string SetGcNumber { get; set; }
        public virtual string SetBillNumber { get; set; }
        public virtual decimal? SetBillQuantity { get; set; }
        public virtual decimal? SetUnloadQuantity { get; set; }

        public SharedModels.UserInfoModel UserInfo { get; set; }

        [ValidateComplexType]
        public List<GcModel> Gcs { get; set; }
    }

    public class GcModel
    {
        public GcModel()
        {
            UserInfo = new  SharedModels.UserInfoModel();
            EwayBill = new();
        }
        public long? GcID { get; set; }
        public long? GcSetID { get; set; }
        public string GcPrefix { get; set; }
        public long? GcNumber { get; set; }
        [Required]
        public short? GcType { get; set; }
        public string GcTypeName { get; set; }
        [Required]
        public DateTime? BillDate { get; set; } = DateTime.Today;
        public string BillNumber { get; set; }
        [Required][Range(1,double.MaxValue)]
        public decimal? BillQuantity { get; set; }
        public decimal? UnloadedQuantity { get; set; }
        public string PassNumber { get; set; }
        public EwayBillModel EwayBill { get; set; }
        public virtual decimal? Freight { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }


    public class EwayBillModel
    {
        public EwayBillModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
        }
        public long? GcID { get; set; }
        [Required]
        public string EwayBillRef { get; set; }
        [Required]
        public DateTime? EwayBillDate { get; set; } = DateTime.Today;
        public DateTime? ExpireOn { get; set; } = DateTime.Today.AddDays(2);
        public SharedModels.UserInfoModel UserInfo { get; set; }
    }
}
