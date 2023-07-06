using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IMechanicJobService
    {
        MechanicJobModel Update(MechanicJobModel model);
        MechanicJobModel SelectByID(int? ID);
        IEnumerable<MechanicJobModel> SelectByJob(int? JipID);
        int Remove(int? MjID, string UserID);
        IEnumerable<MechanicJobModel> Select(int? MjID);
    }
}