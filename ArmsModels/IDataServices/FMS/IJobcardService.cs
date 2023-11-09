using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IJobcardService
    {
        JobcardModel Update(JobcardModel model);  //Edit
        JobcardModel SelectByID(int? ID);
        int Delete(int? JobcardID, string UserID);  //Delete
        IEnumerable<JobcardModel> Select(int? ID);
        IEnumerable<JobcardModel> SelectByBranch(int? BranchID, bool Active = false);
        IEnumerable<JobcardModel> SelectByTruck(int? TruckID, bool Active = false);
        int AddPurchase(int? JobCardID, int? PID);  //Add
    }
}