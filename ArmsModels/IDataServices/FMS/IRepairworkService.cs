using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IRepairJobService
    {
        RepairJobModel Update(RepairJobModel model);
        RepairJobModel SelectByID(int? ID);
        int Delete(int? RepairJobID, string UserID);
        IEnumerable<RepairJobModel> Select(int? RepairJobID);
    }
}