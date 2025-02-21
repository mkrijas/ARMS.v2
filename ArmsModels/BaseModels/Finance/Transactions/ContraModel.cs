using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class ContraModel : TransactionBaseModel, ICloneable
    {
        // Model representing a contra transaction
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ContraModel>(Json);
        }
        public int? ContraID { get; set; } // Unique identifier for the contra transaction
        [Required]
        public string ContraModeHome { get; set; }
        [Required]
        public string ArdCodeHome { get; set; }
        [Required]
        public int? CoaIDHome { get; set; }
        [Required]
        public string ContraModeOther { get; set; }
        [Required]
        public string ArdCodeOther { get; set; }
        [Required]
        public int? CoaIDOther { get; set; }
        // Custom validation attribute to require PaymentTool if ContraModeHome is "Bank"
        [RequiredIf("ContraModeHome", "Bank",ErrorMessage = "Please select Payment Tool!")]        
        public string PaymentTool { get; set; }
        public decimal? BankCharges { get; set; }
        // Custom validation attribute to require ChequeInfo if PaymentTool is "CHEQUE"
        [RequiredIf("PaymentTool", "CHEQUE", ErrorMessage = "Cheque details must be specified")]
        public ChequeModel ChequeInfo { get; set; } = new(); // Information about the cheque used for the transaction
        public string EntryReference { get; set; }
        public bool IsPayment { get; set; }
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        public bool? IsTdsApplicable { get; set; }
        public decimal? TdsAmount { get; set; }
    }
}