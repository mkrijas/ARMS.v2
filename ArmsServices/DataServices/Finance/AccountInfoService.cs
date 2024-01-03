using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using Core.BaseModels.Finance;

namespace ArmsServices.DataServices
{
    public class AccountInfoService : IAccountInfoService
    {
        IDbService Iservice;

        public AccountInfoService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public AccountInfoViewModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MID", ID),
               new SqlParameter("@Operation", "Main"),
            };
            AccountInfoViewModel model = new AccountInfoViewModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.finance.Transactions.Main.Select]", parameters))
            {
                model = GetModel(dr,ID);
            }
            return model;
        }

        public IEnumerable<AccountInfoViewSubModel> Entries(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Sub"),
               new SqlParameter("@MID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.finance.Transactions.Main.Select]", parameters))
            {
                yield return new AccountInfoViewSubModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    AccountName =  dr.GetString("AccountName"),
                    BranchName = dr.GetString("BranchName"),
                    Reference = dr.GetString("EntryReference"),
                    CostCenter = dr.GetString("CostCenter"),
                    Dimension = dr.GetString("Dimension")
                };
            }
        }

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
                model = new CancellationReasonCodesByDocumentType
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
                model = new CancellationReasonCodesByDocumentType
                {
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
                };
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

        private AccountInfoViewModel GetModel(IDataRecord dr,int? MID)
        {
            return new AccountInfoViewModel()
            {               
               DocumentDate = dr.GetDateTime("DocDate"),
               DocumentNumber = dr.GetString("DocNumber"),
               Narration = dr.GetString("Narration"),
               Entries = Entries(MID).ToList()
            };
        }
   
       
    }
}

