using ArmsServices;
using Core.BaseModels.Finance.LedgerViews;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using Core.IDataServices.Finance.LedgerViews;

namespace DAL.DataServices.Finance.LedgerViews
{
    public class BankLedgerViewService: IBankLedgerViewService
    {
        IDbService Iservice;
        public BankLedgerViewService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Retrieves bank ledger records based on Bank ID and date range.
        public IEnumerable<LedgerViewsModel> SelectByBankIDAndDate(int? BankID, DateTime? FromDate, DateTime? ToDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BankID", BankID),
               new SqlParameter("@FromDate", FromDate),
               new SqlParameter("@ToDate", ToDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[rptFinanceReportLedgerSelectByArdCodeBank]", parameters))
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