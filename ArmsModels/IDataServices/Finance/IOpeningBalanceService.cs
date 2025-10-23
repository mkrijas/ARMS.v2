using System;
using System.Collections.Generic;
using ArmsModels.BaseModels;

namespace Core.IDataServices.Finance
{
    public interface IOpeningBalanceService
    {
        //IEnumerable<OpeningBalanceModel> Select();
        IEnumerable<PeriodModel> GetPeriods();
        IEnumerable<OpeningBalanceModel> GetBalance(int? PeriodID, int? BranchID);
        OpeningBalanceModel Update(OpeningBalanceModel model);
        IEnumerable<OpeningBalanceModel> GetBalanceByArd(string BranchIDS, string ArdCode, DateTime Date);
        public int Reset(int? PeriodID, string UserID);

    }
}