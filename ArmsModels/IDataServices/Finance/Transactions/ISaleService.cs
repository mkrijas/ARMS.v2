
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ISaleService
    {
        SaleModel Update(SaleModel model);  //Edit
        SaleModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<SaleModel> Select();
        IEnumerable<SaleModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm, string Type);
        IEnumerable<SaleModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm, string Type);
        IEnumerable<SaleModel> SelectByParty(int? PartyID);
        IEnumerable<SaleModel> SelectByPeriod(DateTime? begin, DateTime? end);
        IEnumerable<TaxPurchaseExpenseModel> GetParticulars(int? SID);
        IEnumerable<TaxPurchaseItemModel> GetItems(int? PID);
        IEnumerable<AssetPOModel> GetAssets(int? PID);
        int Approve(int? SID, string UserID, string Remarks);  //Approve
        int Reverse(int? SID, string UserID);  //Reverse
    }
}