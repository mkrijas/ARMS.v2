using Core.BaseModels.Operations;
using System.Collections.Generic;

namespace Core.IDataServices.Operations
{
    public interface ITruckAvailabilityService
    {
        RequestApprovalHistoryModel UpdateOutgoing(RequestApprovalHistoryModel model);
        IEnumerable<RequestApprovalHistoryModel> SelectOutgoingRequests(int? ID, int? Branch);
        int DeleteRequest(int? ID, string UserID);
        RequestApprovalHistoryModel UpdateStatus(RequestApprovalHistoryModel model);
        IEnumerable<RequestApprovalHistoryModel> SelectIncomingTrucks(int? ID, int? BranchID);
    }
}
