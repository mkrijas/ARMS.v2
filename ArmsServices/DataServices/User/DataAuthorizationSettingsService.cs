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

        // Method to delete a data authorization setting
        public int? Delete(int? DocTypeID,int? AuthLevelID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocTypeID", DocTypeID),
               new SqlParameter("@AuthLevelID", AuthLevelID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.User.DataAuthorization.Settings.Delete]", parameters);
        }

        // Method to get all authorization types
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

        // Method to get document types as a dictionary
        public IDictionary<int?, string> GetDocTypes()
        {
            Dictionary<int?, string> DocTypes = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DocTypes.Select]", null))
            {
                DocTypes.Add(dr.GetInt32("ID"), dr.GetString("Description"));
            }
            return DocTypes;
        }

        // Method to get a list of document types
        public IEnumerable<DocTypeModel> GetDocTypesList()
        {            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DocTypes.Select]", null))
            {
                yield return new DocTypeModel() 
                {
                Description= dr.GetString("Description"),
                ID = dr.GetInt32("ID"),
                AuthImplemented = dr.GetBoolean("AuthImplemented")
                };
            }           
        }

        // Method to select data authorization settings by document type ID
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

        // Method to select data authorization settings by document typec
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

        // Method to update a data authorization setting
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

        // Private method to convert an IDataRecord to a DataAuthorizationSettingsModel
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