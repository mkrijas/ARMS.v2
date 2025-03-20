using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public interface IAssetTransferService
    {
        AssetTransferInitiationModel UpdateOutgoing(AssetTransferInitiationModel model);  //Edit
        IEnumerable<AssetTransferInitiationModel> SelectOutgoingAssets(int? Branch);
        IEnumerable<AssetTransferInitiationModel> SelectOutgoingApproved(int? Branch, int? NumberOfRecords, string searchTerm);
        IEnumerable<AssetTransferInitiationModel> SelectOutgoingUnapproved(int? Branch, int? NumberOfRecords, string searchTerm);
        IEnumerable<AssetSettingsModel> GetCheckList(int? ID);
        IEnumerable<AssetTransferItemModel> GetAssets(int? ID);
        int DeleteInitiation(int? ID, int? BranchID, int? AssetID, string UserID);  //Delete
        AssetTransferInitiationModel UpdateStatus(AssetTransferInitiationModel model, List<int?> RecievedList);  //Accept  //Reject
        IEnumerable<AssetTransferInitiationModel> SelectIncomingAssets(int? Branch);
        IEnumerable<AssetTransferInitiationModel> SelectIncomingApproved(int? Branch, int? NumberOfRecords, string searchTerm);
        IEnumerable<AssetTransferInitiationModel> SelectIncomingUnapproved(int? Branch, int? NumberOfRecords, string searchTerm);
        int RemovePhoto(AssetTransferInitiationModel model);
        public int ApproveOutgoing(int? ID, string UserID);
        public int ApproveIncoming(int? ID, string UserID);
    }
}