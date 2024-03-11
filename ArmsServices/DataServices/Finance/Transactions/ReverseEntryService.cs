using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using ArmsModels.BaseModels.Finance.Transactions;

namespace ArmsServices.DataServices
{
    public class ReverseEntryService : IReverseEntryService
    {
        IDbService Iservice;

        public ReverseEntryService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ReverseEntryID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transaction.Reverse.Delete]", parameters);
        }

        public IEnumerable<CancellationReasonCodesByDocumentType> Select()
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),

            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transaction.Reverse.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<CancellationReasonCodesByDocumentType> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transaction.Reverse.Select]", parameters))
            {
                yield return GetModel(dr);
            }

        }

        public IEnumerable<CancellationReasonCodesByDocumentType> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transaction.Reverse.Select]", parameters))
            {
                yield return GetModel(dr);
            }

        }

        public CancellationReasonCodesByDocumentType SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ReverseEntryID", ID),
               new SqlParameter("@Operation", "GetEntries")
            };
            CancellationReasonCodesByDocumentType model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transaction.Reverse.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public int Approve(int? ReverseEntryID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ReverseEntryID", ReverseEntryID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transaction.Reverse.Approve]", parameters);
        }

        public CancellationReasonCodesByDocumentType Update(CancellationReasonCodesByDocumentType model)
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

        public CancellationReasonCodesByDocumentType GetReverseEntryDetailsByDocumentTypeAndDocumentID(int? DocumentID, int? DocumentTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentID", DocumentID),
               new SqlParameter("@DocumentTypeID", DocumentTypeID),
               new SqlParameter("@Operation", "ByDocumentIDAndDocumentTypeID"),
            };
            CancellationReasonCodesByDocumentType model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transaction.Reverse.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
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

        private CancellationReasonCodesByDocumentType GetModel(IDataRecord dr)
        {
            return new CancellationReasonCodesByDocumentType
            {
                ReverseEntryID = dr.GetInt32("ReverseEntryID"),
                DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                DocumentTypeName = dr.GetString("DocumentTypeName"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                DocumentID = dr.GetInt32("DocumentID"),
                MID = dr.GetInt32("MID"),
                Remarks = dr.GetString("Remarks"),
                ReasonCodeID = dr.GetInt32("ReasonCodeID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
            };
        }

        public IEnumerable<CancellationReasonCodesByDocumentType> Select(int? BranchID)
        {
            throw new NotImplementedException();
        }

        public int Reverse(int? ReverseEntryID, string UserID, string Remarks)
        {
            throw new NotImplementedException();
        }
    }

}
