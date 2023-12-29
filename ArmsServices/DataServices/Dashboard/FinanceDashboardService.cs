using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Data;

namespace ArmsServices.DataServices
{
    public class FinanceDashboardService : IFinanceDashboardService
    {
        IDbService Iservice;
        public FinanceDashboardService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public FinanceDashboardModel GetAccountBalance(int? CoaID, int? BranchID, DateTime? Date)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CoaID", CoaID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Date",Date)
            };
            FinanceDashboardModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.AccountBalance.Select]", parameters))
            {
                model = GetTotalAmount(dr);
            }
            return model;
        }

        private FinanceDashboardModel GetTotalAmount(IDataRecord dr)
        {
            return new FinanceDashboardModel
            {
                CoaID = dr.GetInt32("CoaID"),
                Date = dr.GetDateTime("Date"),
                TotalAmount = dr.GetDecimal("Result")
            };
        }
    }
}