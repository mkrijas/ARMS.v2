using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IPaymentService : IbaseInterface<PaymentMemoModel>
    {       
        IEnumerable<PaymentMemoModel> SelectInterBranch(int? BranchID);
        IEnumerable<PaymentMemoModel> SelectInterBranchByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<PaymentMemoModel> SelectInterBranchByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<PaymentMemoModel> SelectByParty(int? PartyID, int? BranchID);
        IEnumerable<PaymentMemoModel> SelectByPeriod(DateTime? begin, DateTime? end, int? BranchID);
        IEnumerable<PaymentMemoModel> SelectInitiated(int? PaymentInitiatedID);
        IEnumerable<PaymentMemoModel> SelectPending(int? BranchID);
        IEnumerable<BillsPaidModel> GetBills(int? PID);
    }
}