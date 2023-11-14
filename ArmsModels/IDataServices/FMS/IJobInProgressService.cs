using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IJobInProgressService
    {
        JobInProgressModel Update(JobInProgressModel model);  //Edit
        JobInProgressModel SelectByID(int? ID);
        int Delete(int? JipID, string UserID);  //Delete
        IEnumerable<JobInProgressModel> Select(int? JipID);
        IEnumerable<JobInProgressModel> SelectByJobcard(int? JobcardID);
    }
}