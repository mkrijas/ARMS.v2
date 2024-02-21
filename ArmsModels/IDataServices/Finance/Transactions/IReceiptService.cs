using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IReceiptService : IbaseInterface<ReceiptModel>
    {
        //ReceiptModel Update(ReceiptModel model);  //Edit
        //ReceiptModel SelectByID(int? ReceiptID);         
        IEnumerable<ReceiptModel> SelectInterBranch(int? BranchID);
        IEnumerable<ReceiptModel> SelectInterBranchByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<ReceiptModel> SelectInterBranchByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<ReceiptModel> SelectByParty(int? PartyID, int? PartyBranchID, int? BranchID);
        IEnumerable<ReceiptModel> SelectByPeriod(DateTime? begin, DateTime? end, int? BranchID);
        IEnumerable<BillsReceiptModel> GetBills(int? ReceiptID);        
        int Reverse(int? PID, string UserID);  //Reverse
        //IEnumerable<PaymentEntryModel> GetPaymentEntries(int? PfID);
    }
}