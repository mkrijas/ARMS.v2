
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ISaleService : IbaseInterface<SaleModel>
    {       
        IEnumerable<SaleModel> Select();
        IEnumerable<SaleModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm, string Type);
        IEnumerable<SaleModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm, string Type);
        IEnumerable<SaleModel> SelectByParty(int? PartyID);
        IEnumerable<SaleModel> SelectByPeriod(DateTime? begin, DateTime? end);
        IEnumerable<TaxPurchaseExpenseModel> GetParticulars(int? SID);
        IEnumerable<TaxPurchaseItemModel> GetItems(int? PID);
        IEnumerable<AssetSaleModel> GetAssets(int? PID);        
        int Reverse(int? SID, string UserID);  //Reverse
        PagedResult<SaleModel> SelectAll(int? BranchID, int page, int pageSize, string search, bool _IsApproved, string SaleType);

    }
}