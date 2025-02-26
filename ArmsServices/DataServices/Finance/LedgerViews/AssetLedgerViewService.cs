using ArmsServices;
using Core.BaseModels.Finance.LedgerViews;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using Core.IDataServices.Finance.LedgerViews;

namespace DAL.DataServices.Finance.LedgerViews
{
    public class AssetLedgerViewService: IAssetLedgerViewService
    {
        IDbService Iservice;
        public AssetLedgerViewService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Retrieves asset ledger entries based on Asset ID and Date range.
        public IEnumerable<LedgerViewsModel> SelectByAssetIDAndDate(int? AssetID, DateTime? FromDate, DateTime? ToDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", AssetID),
               new SqlParameter("@FromDate", FromDate),
               new SqlParameter("@ToDate", ToDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[rptFinanceReportLedgerSelectByArdCodeAsset]", parameters))
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