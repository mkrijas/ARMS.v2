using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{

    public interface IDataAuthorizationService
    {
        DataAuthorizationModel Update(DataAuthorizationModel model);
        DataAuthorizationModel SelectByID(int? ID);
        IEnumerable<DataAuthorizationModel> SelectByDocument(int? DocTypeID, int? DocumentID);
        IEnumerable<DataAuthorizationModel> SelectByDocument(string DocType, int? DocumentID);
        int Delete(int? ID, string UserID);
        IEnumerable<DataAuthorizationModel> GetAuthStatus(int? DocTypeID, int? DocumentID);
    }
}