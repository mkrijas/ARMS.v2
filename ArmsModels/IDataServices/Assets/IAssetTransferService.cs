using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public interface IAssetTransferService
    {
        AssetTransferInitiationModel UpdateOutgoing(AssetTransferInitiationModel model);
        IEnumerable<AssetTransferInitiationModel> SelectOutgoingAssets(int? Branch);
        int DeleteInitiation(int? ID, int? BranchID, int? AssetID);
        AssetTransferInitiationModel UpdateStatus(AssetTransferInitiationModel model);
        IEnumerable<AssetTransferInitiationModel> SelectIncomingAssets(int? Branch);

        ChildAssetTransferInitiationModel UpdateOutgoingChild(ChildAssetTransferInitiationModel model);
        IEnumerable<ChildAssetTransferInitiationModel> SelectOutgoingChildAssets(int? Branch);
        int DeleteInitiationChild(int? ID, int? BranchID, int? AssetID);
        ChildAssetTransferInitiationModel UpdateStatusChild(ChildAssetTransferInitiationModel model);
        IEnumerable<ChildAssetTransferInitiationModel> SelectIncomingChildAssets(int? Branch);
    }
}