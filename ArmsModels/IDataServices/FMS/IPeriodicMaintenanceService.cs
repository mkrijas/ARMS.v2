using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IPeriodicMaintenanceService
    {
        PeriodicMaintenanceInitiateModel Update(PeriodicMaintenanceInitiateModel model);  //Edit
        PeriodicMaintenanceInitiateModel SelectByID(int? ID);
        int Delete(int? PMIID, string UserID);  //Delete
        IEnumerable<PeriodicMaintenanceInitiateModel> Select(int? PMIID);
        PeriodicMaintenanceConcludeModel Conclude(PeriodicMaintenanceConcludeModel model);  //Edit
        IEnumerable<PeriodicMaintenanceInitiateModel> SelectByTruck(int? TruckID, int? RecordStatus);
    }
}