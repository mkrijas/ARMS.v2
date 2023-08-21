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
        AssetModel UpdateAsset(AssetModel model);        
        int MoveAsset(int? AssetID,int? ParentAssetID,string Mode, string UserID);
        AssetModel SelectByID(int? ID);
        int? Scrap(int? AssetID, string UserID);
        IEnumerable<AssetModel> SelectByBranch(int BranchID,bool scrap);
        IEnumerable<AssetModel> SelectBySubClass(int BranchID, int? SubClassID);
        IEnumerable<AssetModel> GetAttachedAssets(int? AssetID);
        IEnumerable<AssetModel> SelectLinkedAssetsOnTruck();
        int? UpdateStatus(AssetStatusUpdateModel model); 
        AssetStatusUpdateModel GetCurrentStatus(int? AssetID);
        IEnumerable<AssetStatusUpdateModel> GetStatusHistory(int? AssetID);
        List<AssetViewModel> GetAssetView(int BranchID,int? parantID);
        int? GetCapitalizationCoaID(int? AssetID);
        int? GetCWIPCoaID(int? AssetID);
        int? GetDepreciationCoaID(int? AssetID);
        int? GetAccumulatedDepreciationCoaID(int? AssetID);
        int? GetRevaluationCoaID(int? AssetID);
        int? GetRevaluationReserveCoaID(int? AssetID);

    }
}
   

     