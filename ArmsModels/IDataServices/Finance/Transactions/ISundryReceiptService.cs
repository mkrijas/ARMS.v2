using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface ISundryReceiptService : IbaseInterface<SundryReceiptModel>
    {
        SundryReceiptModel Update(SundryReceiptModel model);  //Edit
        SundryReceiptModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<SundryReceiptModel> Select();
        IEnumerable<SundryReceiptModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<SundryReceiptModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<SundryReceiptEntryModel> GetEntries(int? SID);
        int Approve(int? SundryReceiptID, string UserID, string Remarks);  //Approve
    }
}