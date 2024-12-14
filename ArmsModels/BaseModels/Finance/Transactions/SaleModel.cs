using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class SaleModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<SaleModel>(Json);
        }
        public int? SID { get; set; }
        public PartyModel PartyInfo { get; set; }
        [Required]
        public bool IsCredit { get; set; } = true;
        public decimal? AdditionalTCS { get; set; }
        public string SalesType { get; set; }
        public string InvoiceNo { get; set; }
        [ValidateComplexType]
        public List<TaxPurchaseExpenseModel> Particulars { get; set; } = new();
        [ValidateComplexType]
        public List<TaxPurchaseItemModel> Items { get; set; } = new();
        public List<AssetSaleModel> Assets { get; set; } = new();
    }
}