using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IFinanceDashboardService
    {
        FinanceDashboardModel GetAccountBalance(int? CoaID, int? BranchID, DateTime? Date);
    }
}