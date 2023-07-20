using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IDistrictService
    {
        DistrictModel Update(DistrictModel model);  //Edit
        int Delete(int? DistrictID, string UserID);  //Delete
        IEnumerable<DistrictModel> Select(int? DistrictID);
        IEnumerable<StateModel> GetStates();
    }
}