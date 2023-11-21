using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface ITdsThresholdLimitService
    {
        TdsThresholdLimitModel Update(TdsThresholdLimitModel model);  //Edit
        TdsThresholdLimitModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<TdsThresholdLimitModel> Select();
        IEnumerable<TdsThresholdLimitModel> SelectByNP(int TdsNPID, DateTime? EntryDate);
    }
}