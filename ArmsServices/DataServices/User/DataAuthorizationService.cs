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
        IEnumerable<DataAuthorizationStatusModel> GetAuthStatus(int? DocTypeID, int? DocumentID);
    }
    public class DataAuthorizationService : IDataAuthorizationService
    {
        IDbService Iservice;
        IDataAuthorizationSettingsService _settings
        public DataAuthorizationService(IDbService iservice, IDataAuthorizationSettingsService settings)
        {
            Iservice = iservice;
            _settings = settings;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.User.DataAuthorization.Delete]", parameters);
        }

        public IEnumerable<DataAuthorizationStatusModel> GetAuthStatus(int? DocTypeID, int? DocumentID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocTypeID",DocTypeID ),
               new SqlParameter("@DocumentID",DocumentID ),
               new SqlParameter("@Operation","ByDocTypeID"),
            };

            List<DataAuthorizationModel> DA = SelectByDocument(DocTypeID,DocumentID).ToList();
            List<DataAuthorizationSettingsModel> DS =  _settings.Select(DocTypeID).ToList();

            foreach (var item in DS)
            {
                yield return new DataAuthorizationStatusModel()
                {
                    AuthLevelID = item.AuthLevelID,
                    AuthType = item.AuthorizeType,
                    DocTypeID = item.DocTypeID,
                    DocumentID = DocumentID,
                    UserInfo = DA.FirstOrDefault(x => x.DocTypeID == item.DocTypeID && x.AuthLevelID == item.AuthLevelID).UserInfo,
                    IsCompleted = DA.Exists(x => x.DocTypeID == item.DocTypeID && x.AuthLevelID == item.AuthLevelID)
                };
            }
        }

        public IEnumerable<DataAuthorizationModel> SelectByDocument(int? DocTypeID, int? DocumentID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocTypeID",DocTypeID ),
               new SqlParameter("@DocumentID",DocumentID ),
               new SqlParameter("@Operation","ByDocTypeID" ),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DataAuthorization.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<DataAuthorizationModel> SelectByDocument(string DocType, int? DocumentID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocType",DocType ),
               new SqlParameter("@DocumentID",DocumentID ),
               new SqlParameter("@Operation","ByDocType" ),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DataAuthorization.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public DataAuthorizationModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            DataAuthorizationModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DataAuthorization.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public DataAuthorizationModel Update(DataAuthorizationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@DocumentTypeId", model.DocTypeID),
               new SqlParameter("@DocumentID", model.DocumentID),
               new SqlParameter("@AuthLevelID", model.AuthLevelID),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DataAuthorization.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private DataAuthorizationModel GetModel(IDataRecord dr)
        {
            return new DataAuthorizationModel()
            {
                ID = dr.GetInt32("ID"),
                DocumentID = dr.GetInt32("DocumentID"),
                DocTypeID = dr.GetInt32("DocumentTypeId "),
                AuthLevelID = dr.GetInt32("AuthLevelID"),     
                Remarks = dr.GetString("Remarks"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },

            };

        }

    }
}
