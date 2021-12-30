using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITruckDocumentService
    {
        TruckDocumentModel Update(TruckDocumentModel model);
        int SaveFilePath(string link, int? id);
        TruckDocumentModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        int Remove(TruckDocumentModel model);
        IEnumerable<TruckDocumentModel> SelectByPeriod(DateTime? startDate,DateTime? endDate);
        IEnumerable<TruckDocumentModel> SelectWithPast(int? TruckID);
        IEnumerable<TruckDocumentModel> SelectByTruck(int? TruckID);
        IEnumerable<TruckDocumentTypeModel> GetDocumentTypes();
        TruckDocumentTypeModel UpdateDocumentType(TruckDocumentTypeModel model);
        IEnumerable<TruckDocumentModel> ValidatePeriod(TruckDocumentModel model);
        bool IsValid(TruckDocumentModel model,DateTime? DateToCheck);


    }

    public class TruckDocumentService : ITruckDocumentService
    {
        IDbService Iservice;
        public TruckDocumentService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public TruckDocumentModel Update(TruckDocumentModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AttachedDocument", model.AttachedDocument),
               new SqlParameter("@EndDate", model.EndDate),               
               new SqlParameter("@DocumentID", model.DocumentID),
               new SqlParameter("@DocumentTypeID", model.DocumentTypeID),
               new SqlParameter("@StartDate", model.StartDate),                       
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@ReferenceDate", model.ReferenceDate),
               new SqlParameter("@NotificationID", model.NotificationID),              
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Document.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<TruckDocumentModel> ValidatePeriod(TruckDocumentModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ValidatePeriod"),
               new SqlParameter("@EndDate", model.EndDate),               
               new SqlParameter("@DocumentTypeID", model.DocumentTypeID),
               new SqlParameter("@StartDate", model.StartDate),
               new SqlParameter("@TruckID", model.TruckID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Document.Select]", parameters))
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
            return Iservice.ExecuteNonQuery("[usp.Truck.Document.FilePath]", parameters);
        }


        public TruckDocumentModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID) ,
               new SqlParameter("@Operation", "ByID"),
            };
            TruckDocumentModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Document.Select]", parameters))
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
            return Iservice.ExecuteNonQuery("[usp.Truck.Document.Delete]", parameters);
        }


        public int Remove(TruckDocumentModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@DocumentTypeID", model.DocumentTypeID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Truck.Document.RemoveType]", parameters);
        }


        public IEnumerable<TruckDocumentModel> SelectByPeriod(DateTime? startDate, DateTime? endDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ToBeExpired"),
               new SqlParameter("@StartDate", startDate),
               new SqlParameter("@EndDate", endDate),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Document.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<TruckDocumentModel> SelectWithPast(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "WithPast"),
                new SqlParameter("@TruckID", TruckID) ,

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Document.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable <TruckDocumentModel> SelectByTruck(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID) ,
               new SqlParameter("@Operation", "ByTruck"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Document.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        private TruckDocumentModel GetModel(IDataRecord dr)
        {
            return new TruckDocumentModel
            {
                AttachedDocument = dr.GetString("AttachedDocument"),
                EndDate = dr.GetDateTime("EndDate"),                
                DocumentID = dr.GetInt32("DocumentID"),
                DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                ReferenceDate = dr.GetDateTime("ReferenceDate"),
                StartDate = dr.GetDateTime("StartDate"),                
                TruckID = dr.GetInt32("TruckID"),
                NotificationID = dr.GetInt32("NotificationID"),
                DocumentTypeName = dr.GetString("DocumentTypeName"),

                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public IEnumerable<TruckDocumentTypeModel> GetDocumentTypes()
        {           
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.DocumentType.Select]", null))
            {
                yield return new TruckDocumentTypeModel() {                 
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

        public TruckDocumentTypeModel UpdateDocumentType(TruckDocumentTypeModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {               
               new SqlParameter("@BlockAfter", model.BlockAfter),              
               new SqlParameter("@DocumentTypeID", model.DocumentTypeID),
               new SqlParameter("@DocumentTypeName", model.DocumentTypeName),
               new SqlParameter("@WarnBefore", model.WarnBefore),                       
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.DocumentType.Update]", parameters))
            {
                model = new TruckDocumentTypeModel()
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

        public bool IsValid(TruckDocumentModel model, DateTime? DateToCheck)
        {
            if (!(model.StartDate?.Date <= DateToCheck?.Date && model.EndDate?.Date >= DateToCheck?.Date))
            {
                return false;
            }
            return true;
        }
    }
}

