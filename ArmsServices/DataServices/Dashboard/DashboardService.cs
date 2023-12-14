using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Data;

namespace ArmsServices.DataServices
{
    public class DashboardService : IDashboardService
    {
        IDbService Iservice;
        public DashboardService(IDbService iservice, ITariffService tariff)
        {
            Iservice = iservice;
        }
        public List<DashboardModel> SelectChartData(int? BranchID, DateTime? From, DateTime? To)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ConsignmentLoadChart"),
               new SqlParameter("@FromDate",From),
               new SqlParameter("@ToDate",To),
               new SqlParameter("@BranchID", BranchID)
            };
            return GetChartData(parameters);
        }

        public List<DashboardModel> SelectDonutData()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "DriverAvailabilityChart"),
            };
            return GetChartData(parameters);
        }

        private List<DashboardModel> GetChartData(List<SqlParameter> parameters)
        {
            List<DashboardModel> list = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.DashBoard.ChartData.Select]", parameters))
            {
                DashboardModel model = GetDashboardModel(dr);
                list.Add(model);
            }
            return list;
        }
        private DashboardModel GetDashboardModel(IDataRecord dr)
        {
            return new DashboardModel
            {
                Label = dr.GetString("DataLabel"),
                Data = dr.GetInt32("TotalData"),
                BillDate = dr?.GetDateTime("BillDate"),
                Total = dr.GetDecimal("Total"),
            };
        }
    }
}