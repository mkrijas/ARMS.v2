using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{  

    public class ConsolidatedDraftBillModel
    {
        public ConsolidatedDraftBillModel()
        {
            UserInfo = new();
        }
        public int? DraftBillID { get; set; }
        [Required]
        public OrderModel Order { get; set; }
        [Required]
        public TariffTypeModel TariffType { get; set; }        

        [ValidateComplexType]
        public List<GcTariffModel> BookedGCs { get; set; }
               
        [Required]
        public DateTime? DocumentDate { get; set; }
        public string DocumentNumber { get; set; }
        [Required]
        public int? BranchID { get; set; }
        public decimal? TotalAmount { get; set; }      
        public string Narration { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; }        
    }

    public class ProformaInvoiceModel : TransactionBaseModel
    {
        public int? ProformaInvoiceID { get; set; }        
        public int? DraftBillID { get; set; }
        public int? OrderID { get; set; }
        public int? PartyBranchCoa { get; set; }
        public int? TariffTypeID { get; set; }
        public int? TariffTypeCoa { get; set; }
        public string Reference { get; set; }
        public virtual decimal? GstRate { get; set; }
        public decimal? Cgst { get; set; }
        public decimal? Sgst { get; set; }
        public decimal? Igst { get; set; }
    }

    public class BillingModel : ProformaInvoiceModel
    {
        public int? BillingID { get; set; }
    }


    public class GcTariffModel
    {
        public long? GcTariffID { get;}        
        public long? GcID { get; set; }
        public int? TariffID { get; set; }
        public decimal? Amount { get; set; }
        public int? ConsolidatedDraftBillID { get; set; }
    }
}
