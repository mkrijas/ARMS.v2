using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

using System.Diagnostics;


namespace ArmsServices.DataServices
{
    public interface IOpTranService
    {
        OpTranModel Update(OpTranModel model);
        int Delete(long? ID, string UserID);
        IEnumerable<OpTranModel> SelectByTrip(long? TripID);
        IEnumerable<OpTranModel> SelectByApprovedTrip(int? BranchID, long? TripID, int? NumberOfRecords, string searchTerm);
        IEnumerable<OpTranModel> SelectByUnapprovedTrip(int? BranchID, long? TripID, int? NumberOfRecords, string searchTerm);
        IEnumerable<OpTranModel> SelectByJobcard(int? JobcardID);
        OpTranModel SelectByID(long? ID);
        int Approve(int? ID, string UserID, string Remarks);
        int Reverse(int? ID, string UserID, string Remarks);
        IEnumerable<OpTranSubModel> GetExpenses(long? TransactionID);
    }
}
