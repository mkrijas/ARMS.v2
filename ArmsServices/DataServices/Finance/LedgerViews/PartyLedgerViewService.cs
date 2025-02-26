using ArmsModels.BaseModels;
using ArmsServices;
using Core.IDataServices.Finance.LedgerViews;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using Core.BaseModels.Finance.LedgerViews;

namespace DAL.DataServices.Finance.LedgerViews
{
    public class PartyLedgerViewService : IPartyLedgerViewService
    {
        IDbService Iservice;
        public PartyLedgerViewService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Retrieves party ledger records based on Party ID, Branch ID, and date range.
        public IEnumerable<LedgerViewsModel> SelectByPartyIDAndDate(PartyModel Party, int? BranchID, DateTime? FromDate, DateTime? ToDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", Party.PartyID),
               new SqlParameter("@ArdCode", Party.PartyCode),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@FromDate", FromDate),
               new SqlParameter("@ToDate", ToDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[rptFinanceReportLedgerSelectByArdCodeParty]", parameters))
            {
                yield return new LedgerViewsModel()
                {
                    AccountName = dr.GetString("AccountName"),
                    Date = dr.GetDateTime("DocDate"),
                    Narration = dr.GetString("Narration"),
                    Amount = dr.GetDecimal("Amount"),
                    Refference = dr.GetString("Refference"),
                };
            }
        }
    }
}