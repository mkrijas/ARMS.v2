using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITruckTypeService
    {
        TruckTypeModel Update(TruckTypeModel model);  //edit
        int Delete(int? TruckTypeID, string UserID);  //delete
        TruckTypeModel SelectByID(short? TruckTypeID);
        IEnumerable<TruckTypeModel> Select(short? TruckTypeID);
    }
}