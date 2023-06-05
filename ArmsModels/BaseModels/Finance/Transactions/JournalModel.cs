using ArmsModels.SharedModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class JournalModel : TransactionBaseModel
    {
        public JournalModel() { }
        public int? JournalID { get; set; }

        [Required]
        public ChartOfAccountModel Debit { get; set; }
        [Required]        
        public ChartOfAccountModel Credit { get; set; }

        public string Reference { get; set; }
    }
}
