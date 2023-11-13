using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using ArmsModels.BaseModels.Finance.Transactions;

namespace ArmsServices.DataServices
{
    public interface ISundryPaymentService
    {
        SundryPaymentModel Update(SundryPaymentModel model);  //Edit
        SundryPaymentModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<SundryPaymentModel> Select();
        IEnumerable<SundryPaymentModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<SundryPaymentModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<SundryPaymentEntryModel> GetEntries(int? SID);
        int Approve(int? SundryPaymentID, string UserID, string Remarks);  //Approve
        int Reverse(int? SundryPaymentID, string UserID, string Remarks);  //Reverse
    }
}