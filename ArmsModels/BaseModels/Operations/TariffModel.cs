using ArmsModels.SharedModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    public class TariffModel
    {
        public TariffModel()
        {            
            UserInfo = new UserInfoModel();
        }
        public int? TariffID { get; set; }  
        public int? OrderID { get; set; }
        public int? RouteID { get; set; }
        [Required]
        public short? TariffTypeID { get; set; }
        [Required]
        public short? TariffFormulaID { get; set; }
        public int? TariffSign { get; set; }
        [Required]
        public decimal? TariffRate { get; set; }
        public byte? TruckAxles { get; set; }
        public virtual string OrderName { get; set; }
        public virtual string RouteName { get; set; }
        public virtual string Formula { get; set; }
        public virtual string TariffTypeName { get; set; }
        public virtual string Unit { get; set; }
        public string UsageId { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    public class TariffFormulaModel
    {
        public TariffFormulaModel()
        {
            UserInfo = new UserInfoModel();
        }

        public short? FormulaID { get; set; }
        [Required]
        public string Formula { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    public class TariffTypeModel
    {
        public TariffTypeModel()
        {
            UserInfo = new UserInfoModel();
        }
        public short? TariffTypeID { get; set; }
        [Required]
        public string TariffTypeName { get; set; }
        public string TariffGroup { get; set; }
        public string Unit { get; set; }
        public bool AllowMultiple { get; set; } = true;
        public int? TariffSign { get; set; } = 1;
        public string Area { get; set; }
        public int? FinancialAccountID { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }

    public class OperationTransactionModel
    {
        public OperationTransactionModel()
        {
            UserInfo = new();
            Transactions = new();
        }
        public long? TransactionID { get; set; } 
        [Required]
        public string RefType { get; set; }
        [Required]
        public long? RefID { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int? BranchID { get; set; }
        public long? FinanceDocID { get; set; }
        [ValidateComplexType]
        public List<OpTranSubModel> Transactions { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }


    public class OpTranSubModel
    {
        public long? TransactionSubID { get; set; }
        public long? TransactionID { get; set; }
        [Required]
        public short? TariffTypeID { get; set; }
        public int? TariffID { get; set; }
        [Required]
        public DateTime? BillDate { get; set; }
        public string Reference { get; set; }
        public int? Sign { get; set; } = -1;
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal? Amount { get; set; }
        public decimal? AmountAsPerNorms { get; set; }
        public decimal? Quantity { get; set; }
        public long? FinanceTranID { get; set; }
        public virtual string Unit { get; set; }
    }

}
