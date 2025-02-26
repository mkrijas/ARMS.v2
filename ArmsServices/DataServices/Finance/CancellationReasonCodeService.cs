using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Core.BaseModels.Finance;

namespace DAL.DataServices.Finance
{
    public class CancellationReasonCodeService : ICancellationReasonCodeService
    {
        IDbService Iservice;

        public CancellationReasonCodeService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to update cancellation reason codes by document type
        public CancellationReasonCodesByDocumentType Update(CancellationReasonCodesByDocumentType model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentTypeID", model.DocumentTypeID),
               new SqlParameter("@CancellationReasonCodeList", model.CancellationReasonCodeList.ToDataTable()),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Masters.CancelationReasonCodes.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to delete a cancellation reason code
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Masters.CancelationReasonCodes.Delete]", parameters);
        }

        // Method to select all cancellation reason codes
        public IEnumerable<CancellationReasonCodesByDocumentType> Select()
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "All"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Masters.CancelationReasonCodes.Select]", parameters))
            {
                yield return new CancellationReasonCodesByDocumentType()
                {
                    //ReasonCodeID = dr.GetInt32("ReasonCodeID"),
                    DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                    DocumentTypeName = dr.GetString("DocumentTypeName"),
                    //UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    //{
                    //    RecordStatus = dr.GetByte("RecordStatus"),
                    //    TimeStampField = dr.GetDateTime("TimeStamp"),
                    //    UserID = dr.GetString("UserID"),
                    //},

                };
            }
        }

        // Method to select cancellation reason codes by document type ID
        public IEnumerable<CancellationReasonCode> SelectSubById(int? DocumentTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentTypeID", DocumentTypeID),
               new SqlParameter("@Operation", "ByDocumentTypeID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Masters.CancelationReasonCodes.Select]", parameters))
            {
                yield return new CancellationReasonCode()
                {
                    ReasonCodeID = dr.GetInt32("ReasonCodeID"),
                    DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                    ReasonCodeDescription = dr.GetString("ReasonCodeDescription"),

                };
            }
        }

        // Method to update a reverse entry for cancellation reason codes
        public CancellationReasonCodesByDocumentType UpdateReverseEntry(CancellationReasonCodesByDocumentType model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentID", model.DocumentID),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentTypeID", model.DocumentTypeID),
               new SqlParameter("@MID", model.MID),
               new SqlParameter("@ReasonCodeID", model.ReasonCodeID),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transaction.Reverse.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to check if a cancellation reason code has already been reversed
        public bool? IsAlreadyReversed(int? DocumentID, int? DocumentTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentID", DocumentID),
               new SqlParameter("@DocumentTypeID", DocumentTypeID),
            };
            bool? Result = false;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transaction.Reverse.IsExists]", parameters))
            {
                Result = dr.GetBoolean("IsEntryAlreadyExist");
            }
            return Result;
        }

        // Helper method to map data record to CancellationReasonCodesByDocumentType
        private CancellationReasonCodesByDocumentType GetModel(IDataRecord dr)
        {
            return new CancellationReasonCodesByDocumentType
            {
                DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                DocumentTypeName = dr.GetString("DocumentTypeName"),
                ReasonCodeID = dr.GetInt32("ReasonCodeID"),
            
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
