using ArmsModels.SharedModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class JournalModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<JournalModel>(Json);
        }
        public JournalModel() { }
        public int? JournalID { get; set; }        
        public List<JournalSubModel> JournalSubList { get; set; } = new();
    }

    public class JournalSubModel
    {
        public int? JournalSubID { get; set; }
        public int? JournalID { get; set; }
        public int? DebitCoaID { get; set; }
        [Required]
        public virtual ChartOfAccountModel Debit { get; set; }
        public int? CreditCoaID { get; set; }
        [Required]
        public virtual ChartOfAccountModel Credit { get; set; }
        public decimal? Amount { get; set; }
        public string Reference { get; set; }
        public int? CostCenter { get; set; }
        public virtual string CostCenterVal { get; set; }
        public int? Dimension { get; set; }
        public virtual string DimensionVal { get; set; }
    }
}