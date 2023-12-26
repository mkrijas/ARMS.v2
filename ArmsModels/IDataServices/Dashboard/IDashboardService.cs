using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IDashboardService
    {
        List<DashboardModel> SelectChartData(int? BranchID, DateTime? From, DateTime? To);
        List<DashboardModel> SelectDonutData(int? BranchID);
        List<DashboardModel> SelectLineChart(int? BranchID, DateTime? To);
    }
}