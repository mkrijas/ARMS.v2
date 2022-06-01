using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels.Finance.Transactions
{
    public class PartyPaymentMemoModel :TransactionBaseModel
    {
        public int? PaymentMemoID { get; set; }
        public int? PartyBranchID { get; set; }
        public DateTime? DueOn { get; set; }

    }
}
