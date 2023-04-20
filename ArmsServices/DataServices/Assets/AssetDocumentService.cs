using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IAssetDocumentService
    {
        AssetDocumentModel Update(AssetDocumentModel model);
        int SaveFilePath(string link, int? id);
        AssetDocumentModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        int DeleteType(int? ID, string UserID);
        int Remove(AssetDocumentModel model);
        IEnumerable<AssetDocumentModel> SelectByPeriod(DateTime? startDate,DateTime? endDate);
        IEnumerable<AssetDocumentModel> SelectWithPast(int? AssetID);
        IEnumerable<AssetDocumentModel> SelectByAsset(int? AssetID);
        IEnumerable<AssetDocumentTypeModel> GetDocumentTypes();
        AssetDocumentTypeModel UpdateDocumentType(AssetDocumentTypeModel model);
        IEnumerable<AssetDocumentModel> ValidatePeriod(AssetDocumentModel model);
        bool IsValid(AssetDocumentModel model,DateTime? DateToCheck);


    }

    public class AssetDocumentService : IAssetDocumentService
    {
        IDbService Iservice;
        public AssetDocumentService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public AssetDocumentModel Update(AssetDocumentModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AttachedDocument", model.AttachedDocument),
               new SqlParameter("@EndDate", model.EndDate),               
               new SqlParameter("@DocumentID", model.DocumentID),
               new SqlParameter("@DocumentTypeID", model.DocumentType.DocumentTypeID),
               new SqlParameter("@StartDate", model.StartDate),                       
               new SqlParameter("@AssetID", model.Asset.AssetID),
               new SqlParameter("@ReferenceDate", model.InvoiceDate),
               new SqlParameter("@NotificationID", model.NotificationID),              
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<AssetDocumentModel> ValidatePeriod(AssetDocumentModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ValidatePeriod"),
               new SqlParameter("@EndDate", model.EndDate),               
               new SqlParameter("@DocumentTypeID", model.DocumentType.DocumentTypeID),
               new SqlParameter("@StartDate", model.StartDate),
               new SqlParameter("@AssetID", model.Asset.AssetID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public int SaveFilePath(string link, int? id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LINK",link),
               new SqlParameter("@ID", id)
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Document.FilePath]", parameters);
        }


        public AssetDocumentModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID) ,
               new SqlParameter("@Operation", "ByID"),
            };
            AssetDocumentModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Document.Delete]", parameters);
        }

        public int DeleteType(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentTypeID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Document.Type.Delete]", parameters);
        }


        public int Remove(AssetDocumentModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", model.Asset.AssetID),
               new SqlParameter("@DocumentTypeID", model.DocumentType.DocumentTypeID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Document.RemoveType]", parameters);
        }


        public IEnumerable<AssetDocumentModel> SelectByPeriod(DateTime? startDate, DateTime? endDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ToBeExpired"),
               new SqlParameter("@StartDate", startDate),
               new SqlParameter("@EndDate", endDate),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<AssetDocumentModel> SelectWithPast(int? AssetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "WithPast"),
                new SqlParameter("@AssetID", AssetID) ,

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable <AssetDocumentModel> SelectByAsset(int? AssetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", AssetID) ,
               new SqlParameter("@Operation", "ByAsset"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        private AssetDocumentModel GetModel(IDataRecord dr)
        {
            return new AssetDocumentModel
            {
                AttachedDocument = dr.GetString("AttachedDocument"),
                EndDate = dr.GetDateTime("EndDate"),                
                DocumentID = dr.GetInt32("DocumentID"),
                DocumentType = new AssetDocumentTypeModel()
                {
                    DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                    DocumentTypeName = dr.GetString("DocumentTypeName"),
                },
                InvoiceDate = dr.GetDateTime("ReferenceDate"),
                StartDate = dr.GetDateTime("StartDate"),     
                Asset = new AssetModel()
                {
                    AssetID = dr.GetInt32("AssetID"),
                },
                NotificationID = dr.GetInt32("NotificationID"),    
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public IEnumerable<AssetDocumentTypeModel> GetDocumentTypes()
        {           
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.DocumentType.Select]", null))
            {
                yield return new AssetDocumentTypeModel() {                 
                DocumentTypeName = dr.GetString("DocumentTypeName"),
                BlockAfter = dr.GetInt32("BlockAfter"),                
                DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                WarnBefore = dr.GetInt32("WarnBefore"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
        }

        public AssetDocumentTypeModel UpdateDocumentType(AssetDocumentTypeModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {               
               new SqlParameter("@BlockAfter", model.BlockAfter),              
               new SqlParameter("@DocumentTypeID", model.DocumentTypeID),
               new SqlParameter("@DocumentTypeName", model.DocumentTypeName),
               new SqlParameter("@WarnBefore", model.WarnBefore),                       
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.DocumentType.Update]", parameters))
            {
                model = new AssetDocumentTypeModel()
                {                   
                    DocumentTypeName = dr.GetString("DocumentTypeName"),
                    BlockAfter = dr.GetInt32("BlockAfter"),                   
                    DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                    WarnBefore = dr.GetInt32("WarnBefore"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
            return model;
        }

        public bool IsValid(AssetDocumentModel model, DateTime? DateToCheck)
        {
            if (!(model.StartDate?.Date <= DateToCheck?.Date && model.EndDate?.Date >= DateToCheck?.Date))
            {
                return false;
            }
            return true;
        }
    }
}

