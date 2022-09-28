using ArmsModels.SharedModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class OpTranModel:TransactionBaseModel
    {
        public OpTranModel()
        {            
            Transactions = new();
        }
        public int? OpTranID { get; set; }
        public int? CreditCoaID { get; set; }
        public long? TripID { get; set; }
        [ValidateComplexType]
        public List<OpTranSubModel> Transactions { get; set; }
        public virtual TripInfoModel TripInfo { get; set; }     
    }


    public class OpTranSubModel
    {
        public long? OpTranSubID { get; set; }
        public int? OpTranID { get; set; } 
        [Required]
        public int? ExpenseID { get; set; }  
        public string Reference { get; set; }        
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Must have Non-Zero Value {1}")]
        public decimal? Amount { get; set; }        
        public decimal? Quantity { get; set; }        
        public string Unit { get; set; }
    }
}
