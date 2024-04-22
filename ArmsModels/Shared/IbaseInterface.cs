using ArmsModels.BaseModels;
using System.Collections.Generic;

namespace ArmsServices.DataServices
{
    public interface IbaseInterface<T> where T : class
    {
        int Approve(int? ID, string UserID, string Remarks);  //Approve
        int Reverse(int? ID, string UserID, string Remarks);  //Reverse
        int Delete(int? ID, string UserID);  //Delete
        T SelectByID(int? ID);
        IEnumerable<T> Select(int? BranchID);
        IEnumerable<T> SelectByApproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm);
        IEnumerable<T> SelectByUnapproved(int? BranchID, int? NumberOfRecords,bool InterBranch, string searchTerm);
        T Update(T model);
    }
}
