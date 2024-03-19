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
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ContraModel>(Json);
        }
        public int? ContraID { get; set; }
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
        [RequiredIf("ContraModeHome", "Bank",ErrorMessage ="Please select Payment Tool!")]        
        public string PaymentTool { get; set; }
        public decimal? BankCharges { get; set; }
        [RequiredIf("PaymentTool", "CHEQUE", ErrorMessage = "Cheque details must be specified")]
        public ChequeModel ChequeInfo { get; set; } = new();
        public string EntryReference { get; set; }
        public bool IsPayment { get; set; }
        public int? CostCenter { get; set; }
        public int? Dimension { get; set; }
        public bool? IsTdsApplicable { get; set; }
        public decimal? TdsAmount { get; set; }
    }
}