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
    // Model representing a journal entry
    public class JournalModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<JournalModel>(Json);
        }
        public JournalModel() { }
        public int? JournalID { get; set; }
        [MustContain(ErrorMessage = "No account added for journal!")] // Custom validation attribute to ensure at least one account is added
        [ValidateComplexType]
        public List<JournalSubModel> JournalSubList { get; set; } = new(); // List of journal sub-entries
    }

    // Model representing a sub-entry in a journal
    public class JournalSubModel
    {
        private ChartOfAccountModel _coaModel; // Backing field for the Chart of Account model
        public int? JournalSubID { get; set; } // Unique identifier for the journal sub-entry
        public int? JournalID { get; set; }
        [Required]
        public int? CoaID { get; set; }
        // Property to get or set the Chart of Account model
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
        public string ARDCode { get; set; } = null;
        public string SubARDCodeVal { get; set; } = null;
    }
}