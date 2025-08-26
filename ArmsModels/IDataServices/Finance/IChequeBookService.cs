using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IChequeBookService
    {
        ChequeBookModel Update(ChequeBookModel model);  //Edit
        int Delete(int? ChequeBookID, string UserID);  //Delete
        int Approve(int ID, string UserID, string Remarks);
        ChequeBookModel SelectByID(int? ID);
        IEnumerable<ChequeBookModel> SelectByApproved(int? OwnBankAccountID);
        IEnumerable<ChequeBookModel> SelectByUnapproved(int? OwnBankAccountID);
        ChequeBookLeavesModel LeafUpdate(ChequeBookLeavesModel model);
        IEnumerable<ChequeBookLeavesModel> GetAllLeaves(int? ChequeBookID);
        IEnumerable<ChequeBookLeavesModel> GetPendingLeaves(int? ChequeBookID);
        IEnumerable<ChequeBookLeavesModel> GetActiveLeaves(int? ChequeBookID);
        IEnumerable<ChequeBookLeavesModel> GetCashedLeaves(int? ChequeBookID);
        IEnumerable<ChequeBookLeavesModel> GetCancelledLeaves(int? ChequeBookID);
        IEnumerable<ChequeBookLeavesModel> GetDeletedLeaves(int? ChequeBookID);
    }
}

