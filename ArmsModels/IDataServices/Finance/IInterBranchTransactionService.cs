using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IInterBranchTransactionService
    {
        InterBranchAccountMappingModel Update(InterBranchAccountMappingModel model);
        InterBranchAccountMappingModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<InterBranchAccountMappingModel> Select(int? NumberOfRecords, string searchTerm);
        InterBranchAccountMappingModel Select(int? BranchID,int? FundTypeID);
        IEnumerable<InterBranchTransactionTypeModel> GetTypes();
    }
}
