using ArmsModels.BaseModels;
using System.Collections.Generic;

namespace ArmsServices.DataServices
{
    public interface IMileageShortageReceiptService : IbaseInterface<MileageShortageReceiptModel>
    {

        IEnumerable<MileageShortageReceiptModel> Select();
        IEnumerable<MileageShortageReceiptModel> SelectByTripID(long? TripID);
        IEnumerable<MileageShortageReceiptModel> SelectByTransferID(int? RequestApprovalHistoryID);
    }
}