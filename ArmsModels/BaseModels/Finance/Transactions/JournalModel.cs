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
        [MustContain(ErrorMessage = "No account added for journal!")]
        [ValidateComplexType]
        public List<JournalSubModel> JournalSubList { get; set; } = new();
    }

    public class JournalSubModel
    {
        private ChartOfAccountModel _coaModel;
        public int? JournalSubID { get; set; }
        public int? JournalID { get; set; }
        [Required]
        public int? CoaID { get; set; }
        public virtual ChartOfAccountModel Coa { get { return _coaModel; } set { _coaModel = value; CoaID = value?.CoaID; } }
        [Required]
        public int? DrCrType { get; set; }
        public virtual string DrCrText
        {
            get
            {
                if (DrCrType != null && DrCrType == 1)
                {
                    return "Dr";
                }
                else if (DrCrType != null && DrCrType == -1)
                {
                    return "Cr";
                }
                return null;
            }
        }
        [Required]
        public decimal? Amount { get; set; }
        public string Reference { get; set; }
        public int? CostCenter { get; set; }
        public virtual string CostCenterVal { get; set; }
        public int? Dimension { get; set; }
        public virtual string DimensionVal { get; set; }
    }
}