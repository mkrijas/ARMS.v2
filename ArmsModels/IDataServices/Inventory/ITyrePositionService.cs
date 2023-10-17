using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface ITyrePositionService
    {
        IEnumerable<TyrePositionModel> Select(int? TruckType);
        IEnumerable<TyrePositionModel> Select();
        TyrePositionModel Update(TyrePositionModel model, int? TruckTypeID);  //Edit
        int Delete(int? ID, string UserID);  //Delete
    }
}