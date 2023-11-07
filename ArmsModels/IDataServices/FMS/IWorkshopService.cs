using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IWorkshopService
    {
        WorkshopModel Update(WorkshopModel model);  //Edit
        WorkshopModel SelectByID(int? ID);
        int Delete(int? WorkshopID, string UserID);  //Delete
        IEnumerable<WorkshopModel> Select(int? WorkshopID);
    }
}