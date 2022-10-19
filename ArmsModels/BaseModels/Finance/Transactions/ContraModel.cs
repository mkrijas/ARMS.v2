using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class ContraModel: TransactionBaseModel
    {
        public int? ContraID { get; set; }        
        public string ContraModeHome { get; set; }
        public int? CoaIDHome { get; set; }
        public bool IsPayment { get; set; }
        public int? BranchIDOther { get; set; }
        public string ContraModeOther { get; set; }
        public int? CoaIDOther { get; set; }        
        public string PaymentTool { get; set; }
        public ChequeModel ChequeInfo { get; set; } = new();
        public string EntryReference { get; set; }
       
    }
}
