using ArmsModels.BaseModels.FMS;
using ArmsModels.BaseModels.General;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices.FMS
{
    public interface IRoutineCheckListService
    {
        RoutineCheckListModel Update(RoutineCheckListModel model);
        IEnumerable<RoutineCheckListModel> SelectItemByBranch(int? Branch);
        IEnumerable<RoutineCheckListModel> GetLastRoutineCheckListDetailsUsingTruckId(int? TruckId);
        //IEnumerable<AssetModel> GetRequestedDocuments(int? RequestID);
        //AssetDocumentRequestModel SelectDocumentRequest(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<RoutineCheckListMasterModel> ExpireItems(int? BranchID,int?TruckID);
    }
}