using ArmsModels.BaseModels;
using Core.BaseModels.Operations;
using System.Collections.Generic;

namespace Core.IDataServices.Operations
{
    public interface ITruckAvailabilityService
    {
        RequestApprovalHistoryModel UpdateOutgoing(RequestApprovalHistoryModel model);
        IEnumerable<RequestApprovalHistoryModel> SelectOutgoingRequests(int? ID, int? Branch);
        public RequestApprovalHistoryModel GetTransferInfo(long? RequestApprovalHistoryID);
        public RequestApprovalHistoryModel GetTransferByID(long? RequestApprovalHistoryID);
        int DeleteRequest(int? ID, string UserID);
        RequestApprovalHistoryModel UpdateStatus(RequestApprovalHistoryModel model, List<int?> RecievedList);
        RequestApprovalHistoryModel UpdateCheckist(RequestApprovalHistoryModel model);
        IEnumerable<RequestApprovalHistoryModel> SelectIncomingTrucks(int? ID, int? BranchID);
        IEnumerable<int?> GetAllTruckIdsByBranchID(int? BranchID);
        IEnumerable<AssetSettingsModel> GetCheckList(int? ID);
    }
}
