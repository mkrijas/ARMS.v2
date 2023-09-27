using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels.Finance.Transactions;

namespace ArmsServices.DataServices
{
    public interface ISundryReceiptService
    {
        SundryReceiptModel Update(SundryReceiptModel model);
        SundryReceiptModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<SundryReceiptModel> Select();
        IEnumerable<SundryReceiptModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<SundryReceiptModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<SundryReceiptEntryModel> GetEntries(int? SID);
        int Approve(int? SundryReceiptID, string UserID, string Remarks);
    }
}
