using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class InterBranchReimbursementModel : TransactionBaseModel, ICloneable, IValidatableObject
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<InterBranchReimbursementModel>(Json);
        }
        public InterBranchReimbursementModel()
        {
            IsInterBranch = true;
            InterBranchTranID = 0;
        }

        public int? ID { get; set; }
        [ValidateComplexType]
        public List<ReimbursementSubModel> Particulars { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Particulars.Count == 0 )
                yield return new ValidationResult("No Expense to reimburse!");
        }
    }


    public class ReimbursementSubModel
    {
        public int? SubID { get; set; }
        [Required]
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
        public  byte RecordStatus { get; set; }

    }
}
