using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public interface IAssetTransferService
    {
        AssetTransferInitiationModel UpdateOutgoing(AssetTransferInitiationModel model, int? TruckID);
        IEnumerable<AssetTransferInitiationModel> SelectOutgoingAssets(int? Branch);
        IEnumerable<AssetSettingsModel> GetCheckList(int? ID);
        int DeleteInitiation(int? ID, int? BranchID, int? AssetID,int? TruckID, string UserID);
        AssetTransferInitiationModel UpdateStatus(AssetTransferInitiationModel model, int? TruckID, List<int?> RecievedList);
        IEnumerable<AssetTransferInitiationModel> SelectIncomingAssets(int? Branch);
       
    }
}