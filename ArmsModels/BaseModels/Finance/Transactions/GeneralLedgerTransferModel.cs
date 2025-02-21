using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Model representing a general ledger transfer
    public class GeneralLedgerTransferModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<GeneralLedgerTransferModel>(Json);
        }
        public int? LedgerTransferID { get; set; } // Unique identifier for the ledger transfer
        [Required]
        public GstUsageCodeModel UsageCode { get; set; }
        [Required]
        public GstUsageCodeModel OtherUsageCode { get; set; }
        public string Reference { get; set; }
        [Required]
        public int? DrCrType { get; set; }
    }
}