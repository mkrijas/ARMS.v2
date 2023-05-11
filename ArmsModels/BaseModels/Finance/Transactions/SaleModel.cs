using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class SaleModel : TransactionBaseModel
    {
        public int? SID { get; set; } 
        public PartyModel PartyInfo { get; set; }
        [Required]
        public bool IsCredit { get; set; } = true;

        public decimal? AdditionalTCS { get; set; }
        [ValidateComplexType]
        public List<TaxPurchaseExpenseModel> Particulars { get; set; } = new();
        [ValidateComplexType]
        public List<TaxPurchaseItemModel> Items { get; set; } = new();
    }    
}
