using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class DriverSalaryPayableModel : TransactionBaseModel, IValidatableObject, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DriverSalaryPayableModel>(Json);
        }
        public DriverSalaryPayableModel()
        {
            NatureOfTransaction = "TaxPurchase";
        }
        public int? ID { get; set; }
        public DateTime? FromDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DateTime? ToDate { get; set; } = DateTime.Today;

        public List<TaxPurchaseExpenseModel> Expenses { get; set; } = new();
        [ValidateComplexType]
        public List<TaxPurchaseItemModel> Items { get; set; } = new();
        [ValidateComplexType]
        public List<AssetPOModel> Assets { get; set; } = new();
    }
  
}