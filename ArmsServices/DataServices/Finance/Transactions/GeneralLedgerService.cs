using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class GeneralLedgerTransferService : IGeneralLedgerTransferService
    {
        IDbService Iservice;
        public GeneralLedgerTransferService(IDbService iservice)
        {
            Iservice = iservice;
        }
        
        public int Approve(int? ID, string UserID, string remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LedgerTransferID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.GeneralLedgerTransfer.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LedgerTransferID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.GeneralLedgerTransfer.Delete]", parameters);
        }

        public IEnumerable<GeneralLedgerTransferModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.GeneralLedgerTransfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<GeneralLedgerTransferModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", IsInterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.GeneralLedgerTransfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<GeneralLedgerTransferModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", IsInterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.GeneralLedgerTransfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public GeneralLedgerTransferModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LedgerTransferID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            GeneralLedgerTransferModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.GeneralLedgerTransfer.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public GeneralLedgerTransferModel Update(GeneralLedgerTransferModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LedgerTransferID", model.LedgerTransferID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@UsageCode", model.UsageCode),
               new SqlParameter("@OtherBranchID", model.OtherBranchID),
               new SqlParameter("@OtherUsageCode", model.OtherUsageCode),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@Referece", model.Reference),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@IsInterBranch", model.IsInterBranch),
               new SqlParameter("@InterBranchTranID", model.InterBranchTranID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@RecordStatus", model.UserInfo.RecordStatus),
               new SqlParameter("@TimeStamp", model.UserInfo.TimeStampField),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.GeneralLedgerTransfer.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        private GeneralLedgerTransferModel GetModel(IDataRecord dr)
        {
            return new GeneralLedgerTransferModel
            {
                LedgerTransferID = dr.GetInt32("LedgerTransferID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                BranchID = dr.GetInt32("BranchID"),
                OtherBranchName = dr.GetString("OtherBranchName"),
                MID = dr.GetInt32("MID"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                Reference = dr.GetString("Referece"),
                FileName = dr.GetString("FilePath"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                InterBranchTranID = dr.GetInt32("InterBranchTranID"),
                IsInterBranch = dr.GetBoolean("IsInterBranch"),
                OtherBranchID = dr.GetInt32("OtherBranch"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }
        public int Reverse(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LedgerTransferID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.GeneralLedgerTransfer.Reverse]", parameters);
        }

        public int RemoveFile(int? ID, string UserID)
        {
            throw new NotImplementedException();
        }
    }
}