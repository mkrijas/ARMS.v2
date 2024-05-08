using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IReimbursementService : IbaseInterface<InterBranchReimbursementModel>
    {
       // InterBranchReimbursementModel Update(InterBranchReimbursementModel model);  //Edit        
        //InterBranchReimbursementModel SelectByID(int? ID);
       // int Delete(int? ID, string UserID);  //Delete
        //IEnumerable<InterBranchReimbursementModel> Select(int? BranchID);
        //IEnumerable<InterBranchReimbursementModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
       // IEnumerable<InterBranchReimbursementModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
       // int Approve(int? ID, string UserID, string Remarks);  //Approve
       // int Reverse(int? ID, string UserID, string Remarks);  //Reverse
        IEnumerable<ReimbursementSubModel> SelectParticulars(int? ID);
    }
}