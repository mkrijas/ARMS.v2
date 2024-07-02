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
        public DashboardService(IDbService iservice)
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
        
        public List<DashboardModel> SelectDonutData(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "DriverAvailabilityChart"),
               new SqlParameter("@BranchID", BranchID)
            };
            return GetChartData(parameters);
        }

        public List<DashboardModel> SelectLineChart(int? BranchID, DateTime? To)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "FreightStatusGraph"),
               new SqlParameter("@ToDate",To),
               new SqlParameter("@BranchID", BranchID)
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
                DateList = dr?.GetDateTime("DateList"),
                Total = dr.GetDecimal("Total"),
                CumulativeTarget = dr.GetDecimal("CumulativeTarget")
            };
        }

        public IEnumerable<DriverModel> GetDriverStatusByEvent(int? BranchID, string SelectedValue)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "DriverDetails"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@SelectedValue", SelectedValue),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.DashBoard.ChartData.SelectOnClick]", parameters))
            {
                yield return new DriverModel()
                {
                    DriverName = dr.GetString("DriverName")
                };
            }
        }
    }
}