using ArmsModels.BaseModels.General;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices.Operations
{

    public interface IRoutineCheckListMasterService
    {
        RoutineCheckListMasterModel Update(RoutineCheckListMasterModel model);
        IEnumerable<RoutineCheckListMasterModel> SelectItemByBranch(int? Branch);
        //IEnumerable<AssetModel> GetRequestedDocuments(int? RequestID);
        //AssetDocumentRequestModel SelectDocumentRequest(int? ID);
        int Delete(int? ID, string UserID);
    }
}