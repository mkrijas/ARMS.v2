using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection.PortableExecutable;

namespace ArmsServices.DataServices
{
    public interface IDataAuthorizationSettingsService
    {
        IEnumerable<DataAuthorizationTypeModel> GetAuthTypes();
        IDictionary<int?, string> GetDocTypes();
        IEnumerable<DocTypeModel> GetDocTypesList();
        IEnumerable<DataAuthorizationSettingsModel> Select(int? DocTypeID);
        IEnumerable<DataAuthorizationSettingsModel> Select(string DocType);
        int? Update(DataAuthorizationSettingsModel model);
        int? Delete(int? DocTypeID, int? AuthLevelID, string UserID);
    }
}