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
    }
    public class DataAuthorizationService : IDataAuthorizationService
    {
        IDbService Iservice;
        public DataAuthorizationService(IDbService iservice)
        {
            Iservice = iservice;
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

        public IEnumerable<DataAuthorizationModel> SelectByDocument(int? DocTypeID, int? DocumentID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocTypeID",DocTypeID ),
               new SqlParameter("@DocumentID",DocumentID ),
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
               new SqlParameter("@AuthorizationTypeId", model.AuthorizationTypeID),
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
                AuthorizationTypeID = dr.GetInt32("AuthorizationTypeId"),
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
