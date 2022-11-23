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
        IEnumerable<DataAuthorizationSettingsModel> Select(int? DocTypeID);
        IEnumerable<DataAuthorizationSettingsModel> Select(string DocType);
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
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),              
            };
            return Iservice.ExecuteNonQuery("[usp.User.DataAuthorization.Settings.Delete]", parameters);
        }

        public IEnumerable<DataAuthorizationTypeModel> GetAuthTypes()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AuthLevelID", "AuthLevelID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DataAuthorization.Types.Select]", parameters))
            {
            
                yield return new DataAuthorizationTypeModel()
                {
                    AuthLevelID = dr.GetInt32("AuthLevelID"),
                    Description = dr.GetString("Description")
                };
            }
        }

        public IDictionary<int?, string> GetDocTypes()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AuthLevelID", "AuthLevelID"),
            };
            Dictionary<int?, string> DocTypes = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DataAuthorization.Types.Select]", parameters))
            {
                DocTypes.Add(dr.GetInt32("AuthLevelID"), dr.GetString("Description"));                
            }
            return DocTypes;
        }

        public IEnumerable<DataAuthorizationSettingsModel> Select(int? DocTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocTypeID",DocTypeID ),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DataAuthorization.Settings.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<DataAuthorizationSettingsModel> Select(string DocTpye)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocType",DocTpye ),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DataAuthorization.Settings.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public int? Update(DataAuthorizationSettingsModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@AuthLevelID", model.AuthLevelID),
                new SqlParameter("@DocTypeID", model.DocTypeID),
               new SqlParameter("@RoleID", model.RoleID),
                 new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.User.DataAuthorization.Settings.Update]", parameters);
        }

        private DataAuthorizationSettingsModel GetModel(IDataRecord dr)
        {
            return new DataAuthorizationSettingsModel()
            {
                ID = dr.GetInt32("ID"),
                AuthLevelID = dr.GetInt32("AuthLevelID"),
                DocTypeID = dr.GetInt32("DocTypeID"),
                RoleID = dr.GetString("RoleID"),
                AuthorizeType=dr.GetString("AuthorzationType"),
                DocType=dr.GetString("DocType")
            };
        }
    }

       
}
