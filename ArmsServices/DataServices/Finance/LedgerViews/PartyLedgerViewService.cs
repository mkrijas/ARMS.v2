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

        public IEnumerable<PartyLedgerViewModel> SelectByPartyIDAndDate(int? PartyID, DateTime? FromDate, DateTime? ToDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@FromDate", FromDate),
               new SqlParameter("@ToDate", ToDate),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Report.Ledger.SelectByArdCode.Party]", parameters))
            {
                yield return new PartyLedgerViewModel()
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
