
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection;


namespace ArmsServices.DataServices
{
    public interface ITaxPurchaseService : IbaseInterface<TaxPurchaseModel>
    {
        TaxPurchaseModel Update(TaxPurchaseModel model);  //Edit
        TaxPurchaseModel CheckInvoiceDuplication(TaxPurchaseModel model);
        TaxPurchaseModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        //int RemoveFile(int? ID, string UserID);  //Delete
        IEnumerable<TaxPurchaseModel> Select();
        IEnumerable<TaxPurchaseModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm, string Type, string TaxPurchaseType);
        IEnumerable<TaxPurchaseModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm, string Type, string TaxPurchaseType);
        IEnumerable<TaxPurchaseModel> SelectByParty(int? PartyID, int? PartyBranchID);
        IEnumerable<TaxPurchaseModel> SelectByPeriod(DateTime? begin, DateTime? end);
        IEnumerable<TaxPurchaseExpenseModel> GetExpenses(int? PID);
        IEnumerable<TaxPurchaseItemModel> GetItems(int? PID);
        int Approve(int? PID, string UserID, string Remarks);  //Approve
        int Reverse(int? PID, string UserID, string Remarks);  //Reverse
        //TaxPurchaseModel UpdateAssetPO(TaxPurchaseModel model);
        IEnumerable<AssetPOModel> GetAssets(int? PID);
    }
}