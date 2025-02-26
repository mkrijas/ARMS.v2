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

        // Method to approve a General Ledger Transfer
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

        // Method to delete a General Ledger Transfer by ID
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LedgerTransferID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.GeneralLedgerTransfer.Delete]", parameters);
        }

        // Method to select General Ledger Transfers based on Branch ID
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

        // Method to select approved General Ledger Transfers
        public IEnumerable<GeneralLedgerTransferModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@InterBranch", IsInterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.GeneralLedgerTransfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select unapproved General Ledger Transfers
        public IEnumerable<GeneralLedgerTransferModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@InterBranch", IsInterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.GeneralLedgerTransfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a General Ledger Transfer by its 
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

        // Method to update GeneralLedgerTransferModel record
        public GeneralLedgerTransferModel Update(GeneralLedgerTransferModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LedgerTransferID", model.LedgerTransferID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@UsageCode", model.UsageCode.UsageCode),
               new SqlParameter("@OtherBranchID", model.OtherBranchID),
               new SqlParameter("@OtherUsageCode", model.OtherUsageCode.UsageCode),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@DrCrType", model.DrCrType),
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

        // Private method to convert an IDataRecord to a GeneralLedgerTransferModel
        private GeneralLedgerTransferModel GetModel(IDataRecord dr)
        {
            return new GeneralLedgerTransferModel
            {
                LedgerTransferID = dr.GetInt32("LedgerTransferID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocNumber"),
                BranchID = dr.GetInt32("BranchID"),
                OtherBranchID = dr.GetInt32("OtherBranchID"),
                //OtherBranchName = dr.GetString("OtherBranchName"),
                UsageCode = new GstUsageCodeModel
                { 
                    UsageCode = dr.GetString("UsageCode"),
                    Description = dr.GetString("UsageCodeDesc"),
                },
                OtherUsageCode = new GstUsageCodeModel
                {
                    UsageCode = dr.GetString("OtherUsageCode"),
                    Description = dr.GetString("OtherUsageCodeDesc"),
                },
                MID = dr.GetInt32("MID"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                Reference = dr.GetString("Reference"),
                FileName = dr.GetString("FilePath"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                DrCrType = dr.GetInt32("DrCrType"),
                InterBranchTranID = dr.GetInt32("InterBranchTranID"),
                IsInterBranch = dr.GetBoolean("IsInterBranch"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Method to reverse a General Ledger Transfer
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