using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROIMileageService
    {
        ROIMileageModel Update(ROIMileageModel model);
        IEnumerable<ROIMileageModel> Select(int? BranchID);
        ROIMileageModel SelectByID(int? ID);
    }
}