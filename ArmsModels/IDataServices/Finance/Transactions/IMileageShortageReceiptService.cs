using ArmsModels.BaseModels.Finance.Transactions;
using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;

namespace ArmsServices.DataServices
{
    public interface IMileageShortageReceiptService
    {
        MileageShortageReceiptModel Update(MileageShortageReceiptModel model);  //Edit
        MileageShortageReceiptModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<MileageShortageReceiptModel> Select();
        IEnumerable<MileageShortageReceiptModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<MileageShortageReceiptModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        int Approve(int? MileageShortageReceiptID, string UserID, string Remarks);  //Approve
        int Reverse(int? SundryReceiptID, string UserID, string Remarks);  //Reverse
        IEnumerable<MileageShortageReceiptModel> SelectByTripID(long? TripID);
        IEnumerable<MileageShortageReceiptModel> SelectByTransferID(int? RequestApprovalHistoryID);
    }
}