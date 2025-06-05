using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Model representing a sale transaction
    public class SaleModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<SaleModel>(Json);
        }
        public int? SID { get; set; } // Unique identifier for the sale
        public PartyModel PartyInfo { get; set; } // Information about the party associated with the sale
        [Required] 
        public bool IsCredit { get; set; } = true;
        public decimal? AdditionalTCS { get; set; }
        public string SalesType { get; set; }
        public string InvoiceNo { get; set; }
        public int? AddressID { get; set; }
        [ValidateComplexType]
        public List<TaxPurchaseExpenseModel> Particulars { get; set; } = new(); // List of expenses associated with the sale
        [ValidateComplexType]
        public List<TaxPurchaseItemModel> Items { get; set; } = new(); // List of items associated with the sale
        public List<AssetSaleModel> Assets { get; set; } = new(); // List of assets associated with the sale
    }
}