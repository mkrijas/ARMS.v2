using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;

namespace ArmsModels.BaseModels
{
    public class InventoryReleaseModel : TransactionBaseModel,IValidatableObject
    {
        public InventoryReleaseModel()
        {
            NatureOfTransaction = "Release";
        }

        public int? RID { get; set; }
        public int? RequestID { get; set; }// Optional
        public StoreModel Store { get; set; }
        public JobcardModel Jobcard { get; set; } // Optional ,  Active Jobcards
        public TruckModel Truck { get; set; } //Optional

        [ValidateComplexType]
        public List<InventoryItemEntryModel> Items { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Items.Count == 0)
                yield return new ValidationResult("No Items selected!");
        }
    } 
    

    public class InventoryRequestModel 
    {
        public int? RequestID { get; set; }// Primary
        [Required]
        public DateTime? RequestDate { get; set; } = DateTime.Today;
        public string RequestNumber { get; set; }
        public int? BranchID { get; set; }
        public JobcardModel Jobcard { get; set; } // Optional ,  Active Jobcards
        public TruckModel Truck { get; set; } //Optional
        [Required]
        public StoreModel Store { get; set; }// Mandatory
        public string Remarks { get; set; }// Optional
        [ValidateComplexType]
        public List<InventoryItemEntryModel> Items { get; set; } = new();
        public UserInfoModel UserInfo { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Items.Count == 0)
                yield return new ValidationResult("No Items selected!");
        }
    }
}
