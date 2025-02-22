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
    // Model representing an operational transaction
    public class OpTranModel : TransactionBaseModel, ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<OpTranModel>(Json);
        }
        public OpTranModel()
        {
            Transactions = new();
        }
        public int? OpTranID { get; set; } // Unique identifier for the operational transaction
        [Required]
        public string PaymentMode { get; set; } // Cash,Bank req
        [Required]
        public string PaymentArdCode { get; set; } //  Account Rule Defenition Code For Bank Or Cash req 8len
        public virtual int? CreditCoaID { get; set; }
        [Required]
        public string Area { get; set; } // Operation,Maintenenace req
        public long? TripID { get; set; }
        public int? AssetTransferID { get; set; }
        public int? RequestApprovalHistoryID { get; set; }
        public int? TruckID { get; set; }
        public string OpTranType { get; set; }
        public int? JobCardID { get; set; }
        [ValidateComplexType]
        [MustContain(ErrorMessage = "No Expenses Added!")]
        public List<OpTranSubModel> Transactions { get; set; } = new(); // List of operational transaction sub-entries
        public virtual TripInfoModel TripInfo { get; set; }  // Information about the trip associated with the transaction
    }

    // Model representing a sub-entry for an operational transaction
    public class OpTranSubModel
    {
        public long? OpTranSubID { get; set; } // Unique identifier for the operational transaction sub-entry
        public long? OpTranID { get; set; }
        [Required]
        public string ExpenseUsageCode { get; set; }
        public virtual string ExpenseTitle { get; set; }
        public int? CoaID { get; set; }
        public string Reference { get; set; }
        [Required]
        //[Range(1, double.MaxValue, ErrorMessage = "Must have Non-Zero Value {1}")]
        public decimal? Amount { get; set; }
        public decimal? Quantity { get; set; }
        public string Unit { get; set; }
        public int? CostCenter { get; set; }
        public virtual string CostCenterVal { get; set; }
        public int? Dimension { get; set; }
        public bool IsReimbursed { get; set; }
        public virtual string DimensionVal { get; set; }
        public virtual OperationPostingGroupModel OperationPostingGroupModel { get; set; } // Model for the operation posting group
        // Property to get the reimbursement status as a string
        public virtual string Reimbursement
        {
            get
            {
                if(IsReimbursed)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
    }
}