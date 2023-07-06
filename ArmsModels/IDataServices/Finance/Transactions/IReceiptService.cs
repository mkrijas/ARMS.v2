using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IReceiptService
    {
        ReceiptModel Update(ReceiptModel model);
        ReceiptModel SelectByID(int? ReceiptID);
        int Delete(int? ID, string UserID);
        IEnumerable<ReceiptModel> Select(int? BranchID);
        IEnumerable<ReceiptModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<ReceiptModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<ReceiptModel> SelectInterBranch(int? BranchID);
        IEnumerable<ReceiptModel> SelectInterBranchByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<ReceiptModel> SelectInterBranchByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<ReceiptModel> SelectByParty(int? PartyID, int? PartyBranchID, int? BranchID);
        IEnumerable<ReceiptModel> SelectByPeriod(DateTime? begin, DateTime? end, int? BranchID);
        IEnumerable<BillsReceiptModel> GetBills(int? ReceiptID);
        int Approve(int? PID, string UserID, string remarks);
        int Reverse(int? PID, string UserID);
        //IEnumerable<PaymentEntryModel> GetPaymentEntries(int? PfID);
    }
}
