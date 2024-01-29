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
        ChartOfAccountModel Update(ChartOfAccountModel model);  //Edit
        ChartOfAccountModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<ChartOfAccountModel> SelectChildren(int? CoaID);
        List<ChartOfAccountModel> SelectAllChildrenAndItsSub(int? CoaID, string searchString);
        IEnumerable<ChartOfAccountModel> SelectBase();
        IEnumerable<ChartOfAccountModel> FilterSubLedgers(string filterText);
        IEnumerable<ChartOfAccountModel> AllLedgers();
        IEnumerable<ChartOfAccountModel> AllGroups();
        IEnumerable<ChartOfAccountModel> SelectByGroup(int? GroupID);
        IEnumerable<CoaBranchAvailabilityModel> GetAllocatedBranches(int? CoaID);
        bool? IsCostCenterIsMadatoryForGivenCoaID(int? CoaID);
        bool? IsDimensionIsMadatoryForGivenCoaID(int? CoaID);
        void SelectAll(int? CoaID, string UserID);  //UpdateSelect
        void UnSelectAll(int? CoaID, string UserID);  //UpdateSelect
        void AddBranch(CoaBranchAvailabilityModel model);  //UpdateSelect
        void RemoveBranch(CoaBranchAvailabilityModel model);  //UpdateSelect
        IEnumerable<CoaBranchAvailabilityModel> GetSubledgersInBranch(int? BranchID, string filterText);
        IEnumerable<PaymentCodeModel> GetPaymentCodes(int? BranchID, string PaymentMode);
        decimal? GetBalance(int? CoaID,string ArdCode,string SubARdCode,DateTime _date);
    }
}