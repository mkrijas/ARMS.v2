using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IAssetDocumentRequestService
    {
        AssetDocumentRequestModel Update(AssetDocumentRequestModel model); 
        IEnumerable<AssetDocumentRequestModel> SelectPendingByBranch(int? Branch);
        IEnumerable<AssetModel> GetRequestedDocuments(int? RequestID);
        AssetDocumentRequestModel SelectDocumentRequest(int? ID);
        int Delete(int? ID, string UserID);        
    }
}


