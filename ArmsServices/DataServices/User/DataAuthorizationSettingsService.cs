using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{

    public interface IDataAuthorizationSettingsService
    {
        IEnumerable<DataAuthorizationTypeModel> GetAuthTypes();
        IDictionary<int?, string> GetDocTypes();
        IEnumerable<DataAuthorizationSettingsModel> Select(int? DocTypeID);
        IEnumerable<DataAuthorizationSettingsModel> Select(string DocTye);
        int? Update(DataAuthorizationSettingsModel model);
        int? Delete(int? ID);
    }
    public class DataAuthorizationSettingsService : IDataAuthorizationSettingsService
    {
        IDbService Iservice;
        public DataAuthorizationSettingsService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int? Delete(int? ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataAuthorizationTypeModel> GetAuthTypes()
        {
            throw new NotImplementedException();
        }

        public IDictionary<int?, string> GetDocTypes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataAuthorizationSettingsModel> Select(int? DocTypeID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataAuthorizationSettingsModel> Select(string DocTye)
        {
            throw new NotImplementedException();
        }

        public int? Update(DataAuthorizationSettingsModel model)
        {
            throw new NotImplementedException();
        }

        private DataAuthorizationSettingsModel GetModel(IDataRecord dr)
        {
            return new DataAuthorizationSettingsModel()
            {

            };
        }
    }

       
}
