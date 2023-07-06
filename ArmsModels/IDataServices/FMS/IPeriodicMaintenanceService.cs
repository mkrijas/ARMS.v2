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
        PeriodicMaintenanceInitiateModel Update(PeriodicMaintenanceInitiateModel model);
        PeriodicMaintenanceInitiateModel SelectByID(int? ID);
        int Delete(int? PMIID, string UserID);
        IEnumerable<PeriodicMaintenanceInitiateModel> Select(int? PMIID);
        PeriodicMaintenanceConcludeModel Conclude(PeriodicMaintenanceConcludeModel model);
        IEnumerable<PeriodicMaintenanceInitiateModel> SelectByTruck(int? TruckID, int? RecordStatus);
    }
}