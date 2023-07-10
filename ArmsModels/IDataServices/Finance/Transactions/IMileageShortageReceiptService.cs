using ArmsModels.BaseModels.Finance.Transactions;
using Core.BaseModels.Finance.Transactions;
using System.Collections.Generic;

namespace ArmsServices.DataServices
{
    public interface IMileageShortageReceiptService
    {
        MileageShortageReceiptModel Update(MileageShortageReceiptModel model);
        MileageShortageReceiptModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<MileageShortageReceiptModel> Select();
        IEnumerable<MileageShortageReceiptModel> SelectByApproved(int? NumberOfRecords, string searchTerm);
        IEnumerable<MileageShortageReceiptModel> SelectByUnapproved(int? NumberOfRecords, string searchTerm);
        int Approve(int? MileageShortageReceiptID, string UserID, string Remarks);
        int Reverse(int? SundryReceiptID, string UserID, string Remarks);
    }
}
