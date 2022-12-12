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
        IEnumerable<AssetDocumentRequestModel> SelectPending();
        IEnumerable<AssetModel> GetRequestedDocuments(int? RequestID);
        AssetDocumentRequestModel SelectDocumentRequest(int? ID);
        int Delete(int? ID, string UserID);        
    }

    public class AssetDocumentRequestService : IAssetDocumentRequestService
    {
        IDbService Iservice;

        public AssetDocumentRequestService(IDbService iservice)
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
            return Iservice.ExecuteNonQuery("[usp.Asset.DocumentRequest.Delete]", parameters);
        }
      

        public IEnumerable<AssetDocumentRequestModel> SelectPending()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID")
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.DocumentRequest.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public AssetDocumentRequestModel SelectDocumentRequest(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID")
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.DocumentRequest.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }



        public AssetDocumentRequestModel Update(AssetDocumentRequestModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentTypeID", model.DocumentType.DocumentTypeID),
               new SqlParameter("@StartDate", model.StartDate),
               new SqlParameter("@EndDate", model.EndDate),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@Assets", model.Assets.Select(x=> x.AssetID).ToList().ToDataTable() ),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.DocumentRequest.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

      
        private AssetDocumentRequestModel GetModel(IDataRecord dr)
        {
            return new AssetDocumentRequestModel
            {
                ID = dr.GetInt32("ID"),
                BranchID = dr.GetInt32("BranchID"),
                StartDate = dr.GetDateTime("StartDate"),
                EndDate = dr.GetDateTime("EndDate"),
                Remarks = dr.GetString("Remarks"),
                DocumentType = new AssetDocumentTypeModel()
                {
                    DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                    DocumentTypeName = dr.GetString("DocumentTypeName"),
                },
                PaymentMemoID = dr.GetInt32("PaymentMemoID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserId"),
                },
            };
        }

        public IEnumerable<AssetModel> GetRequestedDocuments(int? RequestID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", RequestID)
               new SqlParameter("@Operation", "GetAssets")
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.DocumentRequest.Select]", parameters))
            {
                yield return new AssetModel()
                {
                   AssetID = dr.GetInt32("AssetID"),
                   Description= dr.GetString("Description"),
                   AssetCode = dr.GetString("AssetCode"),                   
                };
            }
        }
    }
}


