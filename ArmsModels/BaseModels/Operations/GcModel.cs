using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArmsModels.BaseModels
{

    public class GcSetModel : ICloneable
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
        public virtual decimal? TotalFreight { get { return Gcs.Sum(x => x.Freight); } }
        public decimal? TotalBillQuantity { get; set; }
        public decimal? TotalUnloadingQuantity { get; set; }
        public long? LoadStartEventID { get; set; }
        public long? LoadEndEventID { get; set; }
        public long? UnloadStartEventID { get; set; }
        public long? UnloadEndEventID { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }

        [ValidateComplexType]
        public List<GcModel> Gcs { get; set; }


        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<GcSetModel>(Json);
        }
    }

    public class GcModel : ICloneable
    {
        public GcModel()
        {
            UserInfo = new SharedModels.UserInfoModel();
            EwayBill = new();
        }
        public long? GcID { get; set; }
        public virtual long? GcSetID { get; set; }
        public virtual string GcPrefix { get; set; }
        public virtual long? GcNumber { get; set; }
        [Required]
        public short? GcType { get; set; }
        public virtual string GcTypeName { get; set; }
        [Required]
        public DateTime? BillDate { get; set; } = DateTime.Today;
        public string BillNumber { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public decimal? BillQuantity { get; set; }
        public virtual decimal? UnloadedQuantity { get; set; }
        public string PassNumber { get; set; }
        public virtual EwayBillModel EwayBill { get; set; }
        [Required]
        public virtual decimal? EFreight { get; set; } = 200;
        [Required]

        public decimal? Freight { get; set; }

        public virtual SharedModels.UserInfoModel UserInfo { get; set; }

        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<GcModel>(Json);
        }
    }


    public class EwayBillModel : ICloneable
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

        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<EwayBillModel>(Json);
        }
    }
}
