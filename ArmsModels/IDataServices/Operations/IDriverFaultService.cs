using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Data;
using System.Data.SqlClient;
namespace ArmsServices.DataServices
{
    public interface IDriverFaultService
    {
        IEnumerable<DriverFaultModel> Select(int? DriverID);
        DriverFaultModel SelectByID(int? FaultID);
        DriverFaultModel Update(DriverFaultModel model);  //Edit
        int Delete(int? DriverID, string UserID);  //Delete
    }
}