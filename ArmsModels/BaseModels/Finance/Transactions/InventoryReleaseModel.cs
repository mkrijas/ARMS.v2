using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class InventoryReleaseModel : TransactionBaseModel, ICloneable    /*, IValidatableObject*/
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InventoryReleaseModel>(Json);
        }
        public InventoryReleaseModel()
        {
            NatureOfTransaction = "Release";
            Narration = "Release from inventory ";
        }
        public int? RID { get; set; }
        public int? RequestID { get; set; }// Optional
        [Required]
        public StoreModel Store { get; set; }
        public JobcardModel Jobcard { get; set; } // Optional ,  Active Jobcards
        public TruckModel Truck { get; set; } //Optional
        public bool IsUsedItem { get; set; }
        public bool IsClosed { get; set; }
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        [ValidateComplexType]
        public List<InventoryItemEntryModel> Items { get; set; } = new();
        public string Status
        {
            get
            {
                if (IsClosed)
                {
                    return "Closed";
                }
                else
                {
                    return "Pending";
                }
            }
        }
    }

    public class InventoryRequestModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InventoryRequestModel>(Json);
        }
        public int? RequestID { get; set; }// Primary
        [Required]
        public DateTime? RequestDate { get; set; } = DateTime.Today;
        public string RequestNumber { get; set; }
        public int? BranchID { get; set; }
        public JobcardModel Jobcard { get; set; } // Optional ,  Active Jobcards
        public TruckModel Truck { get; set; } //Optional
        [Required]
        public StoreModel Store { get; set; }// Mandatory
        public int? RID { get; set; }
        public bool IsClosed {  get; set; }
        public InventoryReleaseSubViewModel ReleaseSubDetails { get; set; }
        public string Remarks { get; set; }// Optional
        [ValidateComplexType]
        public List<InventoryItemEntryModel> Items { get; set; } = new();
        public UserInfoModel UserInfo { get; set; } = new();
        public bool? IsCompletelyReleased { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Items.Count == 0)
                yield return new ValidationResult("No Items selected!");
        }
    }

    public class InventoryReleaseSubViewModel
    {
        public long? ItemEntryID { get; set; }
        [Required]
        public int? ItemID { get; set; }
        public string ItemDescription { get; set; }
        [Required]
        public decimal? ItemQty { get; set; }
        public decimal? RequestQty { get; set; }
        public decimal? AvailableQty { get; set; }
        public decimal? PendingQty { get; set; }
        public decimal? ReleaseQty { get; set; }
        public decimal? UsedQty { get; set; }
    }

    public class ExpenseModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ExpenseModel>(Json);
        }
        public int? JFID { get; set; }// Primary
        public string DocNumber { get; set; }
        public DateTime? DocDate { get; set; }
        public string BranchName { get; set; }
        public string RegName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Narration { get; set; }
        public string UserID { get; set; }
        public string AccountName { get; set; }// Optional
        public decimal? Amount { get; set; }
        public decimal? SGST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? IGST { get; set; }
        public decimal? TDS { get; set; }
        public string BillReference { get; set; }
        public string CostCenter { get; set; }
        public string Dimension { get; set; }
    }

}