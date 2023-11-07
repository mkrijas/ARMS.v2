using ArmsModels.BaseModels.FMS;
using ArmsModels.BaseModels.General;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices.FMS
{
    public interface IRoutineCheckListService
    {
        RoutineCheckListModel Update(RoutineCheckListModel model);  //Edit
        IEnumerable<RoutineCheckListModel> SelectItemByBranch(int? Branch);
        IEnumerable<RoutineCheckListModel> GetLastRoutineCheckListDetailsUsingTruckId(int? ID);
        IEnumerable<RoutineCheckListModel> GetLastRoutineCheckListDetailsUsingID(int? ID);

        //IEnumerable<AssetModel> GetRequestedDocuments(int? RequestID);
        //AssetDocumentRequestModel SelectDocumentRequest(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<RoutineCheckListMasterModel> ExpireItems(int? BranchID,int?TruckID);
    }
}