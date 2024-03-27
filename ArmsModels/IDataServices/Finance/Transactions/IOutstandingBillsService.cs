using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Security.Cryptography;


namespace ArmsServices.DataServices
{
    public interface IOutstandingBillsService
    {
        OutstandingBillsModel SelectByID(int? ID);
        IEnumerable<OutstandingBillsModel> Select(int BranchID);
        IEnumerable<OutstandingBillsModel> SelectByParty(int? PartyID, int? BranchID);
        IEnumerable<OutstandingBillsModel> SelectByPeriod(DateTime? begin, DateTime? end);
        // int SettleBillsToPayment(int? OPID, List<BillsReceiptModel> Bills);
        int? AutoSettle(AutoSettleModel model);  //AutoSettle
        IEnumerable<BillsPaidModel> GetAutoSettledBills(int? ID);
        IEnumerable<AutoSettleModel> SelectAutoSettledEntriesByApproved(int? BranchID, int numberOfRecords);
        IEnumerable<AutoSettleModel> SelectAutoSettledEntriesByUnapproved(int? BranchID,int numberOfRecords);
        int DeleteAutoSettle(int? ID, string userID);
        int Approve(int? ID, string UserID, string Remarks);
    }
}