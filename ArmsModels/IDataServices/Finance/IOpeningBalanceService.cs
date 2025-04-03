using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using ArmsServices.DataServices;

namespace Core.IDataServices.Finance
{
    public interface IOpeningBalanceService 
    {
        //IEnumerable<OpeningBalanceModel> Select();
        IEnumerable<PeriodModel> GetPeriods();
        IEnumerable<OpeningBalanceModel> GetBalance(int? PeriodID, int? BranchID);
        OpeningBalanceModel Update(OpeningBalanceModel model);
        public int Reset(int? PeriodID, string UserID);

    }
}