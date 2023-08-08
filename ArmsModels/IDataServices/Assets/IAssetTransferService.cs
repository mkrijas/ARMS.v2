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
        IEnumerable<AssetSettingsModel> GetCheckList(int? ID);
        int DeleteInitiation(int? ID, int? BranchID, int? AssetID);
        AssetTransferInitiationModel UpdateStatus(AssetTransferInitiationModel model, List<int?> RecievedList);
        IEnumerable<AssetTransferInitiationModel> SelectIncomingAssets(int? Branch);
       
    }
}