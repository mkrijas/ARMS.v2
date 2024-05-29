using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class TdsTransactionModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TdsTransactionModel>(Json);
        }
        public int? ID { get; set; }
      
        [Required]
        public PartyModel Party { get; set; }
        [Required]
        public int? PartyCoaID { get; set; }
        [Required]
        public bool IsTdsPayable { get; set; }

        public List<TdsTransactionEntryModel> Tds { get; set; } = new List<TdsTransactionEntryModel>();
    }


    public class TdsTransactionEntryModel
    {
        public int? ID { get; set; }
        public int? TransactionID { get; set; }
        [Required]
        public int? TdsNpID { get; set; }
        public virtual string TdsNP { get; set; }
        [Required]
        public DateTime? InvoiceDate { get; set; }
        [Required]
        public string InvoiceNumber { get; set; }
        [Required]
        public decimal? TaxableAmount { get; set; }
        [Required]
        public decimal? RateOfTds { get; set; }
        [Required]
        public decimal? TdsAmount { get; set; }       

    }
}