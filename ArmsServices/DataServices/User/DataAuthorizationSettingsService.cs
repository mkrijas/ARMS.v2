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
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DataAuthorization.Types.Select]", null))
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
            Dictionary<int?, string> DocTypes = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DocTypes.Select]", null))
            {
                DocTypes.Add(dr.GetInt32("ID"), dr.GetString("Description"));
            }
            return DocTypes;
        }

        public IEnumerable<DataAuthorizationSettingsModel> Select(int? DocTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocTypeID",DocTypeID ),
               new SqlParameter("@Operation","ByDocTypeID" ),
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
               new SqlParameter("@Operation","ByDocType" ),
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
                ClaimValue = dr.GetString("ClaimValue"),
                AuthorizeType = dr.GetString("AuthorzationType"),
                DocType = dr.GetString("DocType")
            };
        }
    }


}
