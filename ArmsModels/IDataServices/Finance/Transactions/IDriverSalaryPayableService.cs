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
    public interface IDriverSalaryPayableService : IbaseInterface<DriverSalaryPayableModel>
    {
        IEnumerable<DriverSalaryPayableModel> Select();
        IEnumerable<DriverSalaryPayableListModel> GetDetails(int? ID);
        IEnumerable<DriverSalaryPayableListModel> GetLists(int? BranchID, DateTime? FromDate, DateTime? ToDate);
        IEnumerable<DriverSalaryPayableModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<DriverSalaryPayableModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        //TaxPurchaseModel UpdateAssetPO(TaxPurchaseModel model);
    }
}