using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IAssetService
    {
        AssetModel UpdateAsset(AssetModel model);  //Edit
        int Delete(int? ID, string UserID);  //Delete
        int MoveAsset(int? AssetID, int? ParentAssetID, string Mode, string UserID);  //Move
        AssetModel SelectByID(int? ID);
        int? Scrap(int? AssetID, string UserID);
        IEnumerable<AssetModel> SelectByBranch(int BranchID, bool scrap, int? NumberOfRecords, string searchTerm);
        void ClearAssets();
        IEnumerable<AssetModel> SelectBySubClass(int BranchID, int? SubClassID);
        IEnumerable<AssetModel> GetAttachedAssets(int? AssetID);
        IEnumerable<AssetModel> SelectLinkedAssetsOnTruck();
        AssetModel SelectByTruckID(int? TruckID);
        int? UpdateStatus(AssetStatusUpdateModel model);
        AssetStatusUpdateModel GetCurrentStatus(int? AssetID);
        IEnumerable<AssetStatusUpdateModel> GetStatusHistory(int? AssetID);
        List<AssetViewModel> GetAssetView(int BranchID, int? parantID, int? NumberOfRecords, string searchTerm);
        int? GetCapitalizationCoaID(int? AssetID);
        int? GetCWIPCoaID(int? AssetID);
        int? GetDepreciationCoaID(int? AssetID);
        int? GetAccumulatedDepreciationCoaID(int? AssetID);
        int? GetRevaluationCoaID(int? AssetID);
        int? GetRevaluationReserveCoaID(int? AssetID);
    }
}