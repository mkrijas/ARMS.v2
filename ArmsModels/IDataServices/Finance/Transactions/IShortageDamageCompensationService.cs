using ArmsModels.BaseModels;
using ArmsModels.BaseModels.Finance.Transactions;
using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;

namespace ArmsServices.DataServices
{
    public interface IShortageDamageCompensationService
    {
        ShortageDamageCompensationModel Update(ShortageDamageCompensationModel model);  //Edit
        ShortageDamageCompensationModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<ShortageDamageCompensationModel> Select();
        IEnumerable<ShortageDamageCompensationModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<ShortageDamageCompensationModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        int Approve(int? DamageID, string UserID, string Remarks);  //Approve
        int Reverse(int? DamageID, string UserID, string Remarks);  //Reverse
        IEnumerable<ShortageDamageCompensationModel> SelectByGcSetID(long? GcSetID);
        public IEnumerable<TripAdvanceModel> GetDamageReceivables(int? BranchID);
    }
}