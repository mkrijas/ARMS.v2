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
        [Required]
        public string PaymentMode { get; set; } // Cash,Bank req
        [Required]
        public string PaymentArdCode { get; set; } //  Account Rule Defenition Code For Bank Or Cash req 8len
        public virtual int? CreditCoaID { get; set; }
        [Required]
        public string Area { get; set; } // Operation,Maintenenace req
        public long? TripID { get; set; } 
        public int? TruckID { get; set; }
        public int? JobCardID { get; set; }
        [ValidateComplexType]
        public List<OpTranSubModel> Transactions { get; set; }
        public virtual TripInfoModel TripInfo { get; set; }  // select   
    }


    public class OpTranSubModel
    {
        public long? OpTranSubID { get; set; }
        public int? OpTranID { get; set; } 
        [Required]
        public GstUsageCodeModel ExpenseUsageCode { get; set; }  
        public string Reference { get; set; }        
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Must have Non-Zero Value {1}")]
        public decimal? Amount { get; set; }        
        public decimal? Quantity { get; set; }        
        public string Unit { get; set; }           
    }
}
