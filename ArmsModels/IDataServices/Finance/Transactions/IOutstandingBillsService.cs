using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Security.Cryptography;


namespace ArmsServices.DataServices
{
    public interface IOutstandingBillsService : IbaseInterface<AutoSettleModel>
    {        
        IEnumerable<OutstandingBillsModel> Select(int BranchID);
        IEnumerable<OutstandingBillsModel> SelectByParty(int? PartyID, int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<OutstandingBillsModel> SelectByPeriod(DateTime? begin, DateTime? end);
        // int SettleBillsToPayment(int? OPID, List<BillsReceiptModel> Bills);      
        IEnumerable<BillsPaidModel> GetAutoSettledBills(int? ID);        
        IEnumerable<OutStandingBillInfoModel> SelectByDocumentNumber(string DocumentNumber);

    }
}