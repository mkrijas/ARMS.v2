using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IInterBranchMappingService
    {
        InterBranchMappingModel Update(InterBranchMappingModel model);
        int Delete(int? ID, string UserID);
        IEnumerable<InterBranchMappingModel> Select();
        InterBranchMappingModel SelectByID(int ID);
    }

    
}
