using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;



namespace ArmsServices.DataServices
{
    public interface IDrCrNoteService
    {
        DrCrNoteModel Update(DrCrNoteModel model);
        DrCrNoteModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<DrCrNoteModel> Select();
        IEnumerable<DrCrNoteModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<DrCrNoteModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<DrCrNoteModel> SelectByParty(int? PartyID, int? PartyBranchID);
        IEnumerable<DrCrNoteModel> SelectByPeriod(DateTime? begin, DateTime? end);
        IEnumerable<TaxPurchaseExpenseModel> GetExpenses(int? ID);
        IEnumerable<TaxPurchaseItemModel> GetItems(int? ID);
        IEnumerable<BillInfoModel> GetBillInfo(int? BranchID,string DrCrType,int? PartyID,string  DocumentNumberSearchKey);
        IEnumerable<TaxPurchaseExpenseModel> GetBillInfoParticulars(int? ID, string BillType);
        IEnumerable<TaxPurchaseItemModel> GetBillInfoItems(int? ID, string BillType);
        int Approve(int? ID, string UserID,string Remarks);
        int Reverse(int? ID, string UserID,string Remarks);
    }
}