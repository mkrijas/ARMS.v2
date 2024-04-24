using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class SundryMaintenanceModel : TransactionBaseModel, ICloneable
    {
        public int? ID { get; set; }
        [Required]
        public string BreakDownType { get; set; }
        [Required]
        public PartyModel PartyInfo { get; set; }
        [ValidateComplexType]
        [Required]
        public List<SundryMaintenanceEntryModel> Entries { get; set; }
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<SundryMaintenanceModel> (Json);
        }
    }

    public class SundryMaintenanceEntryModel
    {
        public int? ID { get; set; }        
        public int? ParentID { get; set; }
        [Required]
        public int? BranchID { get; set; }
        [Required]
        public int? Job { get; set; }
        public virtual string JobTitle { get; set; }
        [Required]
        public string UsageCode { get; set; }
        public int? CoaID { get; set; }
        public virtual string UsageCodeDescription { get; set; }
        public string SubArdCode { get; set; }
        [Required]
        public int? TruckID { get; set; }
        public virtual string TruckRegNo { get; set; }
        [Required]
        public string InvoiceNo { get; set; }
        [Required]
        public DateTime? InvoiceDate { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public decimal? GstRate { get; set; }
        public decimal? SGST { get; set; } = 0;
        public decimal? CGST { get; set; } = 0;
        public decimal? IGST { get; set; } = 0;
        public decimal? TDS { get; set; } = 0;
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        public virtual string CostCenterVal { get; set; }
        public virtual string DimensionVal { get; set; }
    }
}
