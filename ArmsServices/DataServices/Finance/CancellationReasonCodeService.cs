using ArmsModels.BaseModels;
using ArmsServices;
using Core.IDataServices.Finance;
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

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Masters.CancelationReasonCodes.Delete]", parameters);
        }
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
