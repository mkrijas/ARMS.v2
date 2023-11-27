using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IContraService
    {
        ContraModel Update(ContraModel model);  //Edit
        IEnumerable<ContraModel> SelectInterBranch(int? BranchID);
        IEnumerable<ContraModel> SelectInterBranchByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<ContraModel> SelectInterBranchByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        ContraModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<ContraModel> Select(int? BranchID);
        IEnumerable<ContraModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<ContraModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        int Approve(int? ID, string UserID, string Remarks);  //Approve
        int Reverse(int? ID, string UserID, string Remarks);  //Reverse
    }
}