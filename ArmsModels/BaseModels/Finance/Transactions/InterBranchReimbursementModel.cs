using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    // Model representing an inter-branch reimbursement transaction
    public class InterBranchReimbursementModel : TransactionBaseModel, ICloneable, IValidatableObject
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InterBranchReimbursementModel>(Json);
        }
        // Constructor to initialize default values
        public InterBranchReimbursementModel()
        {
            IsInterBranch = true; // Set IsInterBranch to true by default
            InterBranchTranID = 0; // Initialize InterBranchTranID to 0
        }

        public int? ID { get; set; } // Unique identifier for the reimbursement transaction
        [ValidateComplexType]
        public List<ReimbursementSubModel> Particulars { get; set; } = new(); // List of reimbursement details

        // Method to validate the model
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Particulars.Count == 0)
                yield return new ValidationResult("No Expense to reimburse!");
        }
    }

    // Model representing a sub-entry for reimbursement
    public class ReimbursementSubModel
    {
        public int? SubID { get; set; }  // Unique identifier for the sub-entry
        //[Required]
        public int? ReimbursementID { get; set; }
        [Required]
        public string UsageCode { get; set; }
        public int? CostCenterID { get; set; }
        public int? DimensionID { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public int? drcrType { get; set; } = 1;// debit = 1; credit = -1
        [Required]
        public string UsageCodeOther { get; set; }
        public int? CostCenterOtherID { get; set; }
        public int? DimensionOtherID { get; set; }
        [Required]
        public decimal? GstRate { get; set; }
        public decimal? SGST { get; set; } = 0;
        public decimal? CGST { get; set; } = 0;
        public decimal? IGST { get; set; } = 0;
        public byte RecordStatus { get; set; }


        public string UsageCodeDesc { get; set; }
        public string UsageCodeOtherDesc { get; set; }
        public string? CostCenterDesc { get; set; }
        public string? DimensionDesc { get; set; }
        public string? drcrTypeDesc { get; set; }// debit = 1; credit = -1
        public string? CostCenterOtherDesc { get; set; }
        public string? DimensionOtherDesc { get; set; }

    }
}
