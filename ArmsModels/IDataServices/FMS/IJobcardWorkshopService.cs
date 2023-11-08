using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IJobcardWorkshopService
    {
        JobcardWorkshopModel Update(JobcardWorkshopModel model);  //Edit
        JobcardWorkshopModel SelectByID(int? ID);
        int Delete(int? JwID, string UserID);  //Delete
        IEnumerable<JobcardWorkshopModel> Select(int? JwID);
        IEnumerable<JobcardWorkshopModel> SelectByJobcard(int? ID);
    }
}