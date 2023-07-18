using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IRouteService
    {
        Task<RouteModel> Update(RouteModel model);  //Edit
        Task<RouteModel> SelectByID(int? ID);
        int Delete(int? RouteID, string UserID);  //Edit
        IAsyncEnumerable<RouteModel> Select(int? RouteID);
        IAsyncEnumerable<RouteModel> SelectByOrder(int? OrderID);
    }
}