using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IPaymentService
    {
        PaymentMemoModel Update(PaymentMemoModel model);
        PaymentMemoModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<PaymentMemoModel> Select(int? BranchID);
        IEnumerable<PaymentMemoModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<PaymentMemoModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<PaymentMemoModel> SelectInterBranch(int? BranchID);
        IEnumerable<PaymentMemoModel> SelectInterBranchByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<PaymentMemoModel> SelectInterBranchByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<PaymentMemoModel> SelectByParty(int? PartyID, int? BranchID);
        IEnumerable<PaymentMemoModel> SelectByPeriod(DateTime? begin, DateTime? end, int? BranchID);
        IEnumerable<PaymentMemoModel> SelectInitiated(int? PaymentInitiatedID);
        IEnumerable<PaymentMemoModel> SelectPending(int? BranchID);
        IEnumerable<BillsPaidModel> GetBills(int? PID);
        int Approve(int? PID, string UserID, string Remarks);
        int Reverse(int? PID, string UserID, string Remarks);
    }
}
