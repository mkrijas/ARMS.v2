using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using Core.BaseModels.Operations;


namespace ArmsServices.DataServices
{
    public interface IDestinationFeasibilityCheckerService
    {
        Task<DestinationFeasibilityCheckerModel> Update(DestinationFeasibilityCheckerModel model);  //Edit
        Task<int> Delete(int? ID, string UserID);  //Edit
        IEnumerable<DestinationFeasibilityCheckerModel> Select(int? ID);
        IEnumerable<DestinationFeasibilityCheckerRatesModel> SelectRates(int? ID);
    }
}