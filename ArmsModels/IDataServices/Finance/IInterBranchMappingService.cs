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
        InterBranchMappingModel SelectByID(int? ID);
        InterBranchMappingModel IsEntriesAlreadyExistOrNot(InterBranchMappingModel model);
        int Delete(int? ID, string UserID);
        IEnumerable<InterBranchMappingModel> Select(int? NumberOfRecords, string searchTerm);
        InterBranchMappingModel Select(int? BranchID,int? FundTypeID);
        IEnumerable<InterBranchTransactionTypeModel> GetTypes();
    }
}
