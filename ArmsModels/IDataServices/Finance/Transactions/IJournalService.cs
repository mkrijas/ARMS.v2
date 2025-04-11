using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IJournalService : IbaseInterface<JournalModel>
    {
        JournalModel Update(JournalModel model);  //Edit
        JournalModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<JournalModel> Select(int? BranchID);
        IEnumerable<JournalModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<JournalModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<JournalModel> SelectByPeriod(DateTime? begin, DateTime? end);       
        int Approve(int? ID, string UserID, string Remarks);  //Approve
        int Reverse(int? ID, string UserID, string Remarks);  //Reverse
        public IEnumerable<JournalSubModel> GetSubList(int? JournalID);
    }
}