using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IRepairWorkService
    {
        RepairJobModel Update(RepairJobModel model);  //Edit
        RepairJobModel SelectByID(int? ID);
        int Delete(int? RepairJobID, string UserID);  //Delete
        IEnumerable<RepairJobModel> Select(int? RepairJobID);
        IEnumerable<RepairJobGroup> SelectGroup();
        IEnumerable<RepairJobGroup> SelectSubGroup(int? GroupID);

    }
}