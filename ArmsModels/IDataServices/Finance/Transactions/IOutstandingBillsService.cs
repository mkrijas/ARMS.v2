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
        int? AutoSettle(OutstandingBillsModel model, List<BillsPaidModel> Bills);  //AutoSettle
    }
}