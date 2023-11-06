using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IChartOfAccountService
    {
        ChartOfAccountModel Update(ChartOfAccountModel model);
        ChartOfAccountModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<ChartOfAccountModel> SelectChildren(int? CoaID);
        List<ChartOfAccountModel> SelectAllChildrenAndItsSub(int? CoaID, string searchString);
        IEnumerable<ChartOfAccountModel> SelectBase();
        IEnumerable<ChartOfAccountModel> FilterSubLedgers(string filterText);
        IEnumerable<ChartOfAccountModel> AllLedgers();
        IEnumerable<ChartOfAccountModel> AllGroups();
        IEnumerable<ChartOfAccountModel> SelectByGroup(int? GroupID);
        IEnumerable<CoaBranchAvailabilityModel> GetAllocatedBranches(int? CoaID);
        void SelectAll(int? CoaID, string UserID);
        void UnSelectAll(int? CoaID, string UserID);
        void AddBranch(CoaBranchAvailabilityModel model);
        void RemoveBranch(CoaBranchAvailabilityModel model);
        IEnumerable<CoaBranchAvailabilityModel> GetSubledgersInBranch(int? BranchID, string filterText);
        IEnumerable<PaymentCodeModel> GetPaymentCodes(int? BranchID, string PaymentMode);
    }
}