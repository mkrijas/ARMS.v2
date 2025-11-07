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
    public class SundryPaymentService : ISundryPaymentService
    {
        IDbService Iservice;

        public SundryPaymentService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a sundry payment entry
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "DELETE"),
               new SqlParameter("@SundryPaymentID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryPayment.Delete]", parameters);
        }

        // Method to remove a file associated with a sundry payment entry
        public int RemoveFile(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "REMOVEFILE"),
               new SqlParameter("@SundryPaymentID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryPayment.Delete]", parameters);
        }

        // Method to select all sundry payment entries
        public IEnumerable<SundryPaymentModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryPayment.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select approved sundry payment entries
        public IEnumerable<SundryPaymentModel> SelectByApproved(int? BranchID, int? NumberOfRecords,bool IsInterBranch ,string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", IsInterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryPayment.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select unapproved sundry payment entries   
        public IEnumerable<SundryPaymentModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", IsInterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryPayment.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to get entries associated with a specific sundry payment
        public IEnumerable<SundryPaymentEntryModel> GetEntries(int? SID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetEntries"),
               new SqlParameter("@SundryPaymentID", SID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryPayment.Select]", parameters))
            {
                yield return new SundryPaymentEntryModel()
                {
                    ID = dr.GetInt32("ID"),
                    ParentID = dr.GetInt32("ParentID"),
                    BranchID = dr.GetInt32("BranchID"),
                    CoaID = dr.GetInt32("CoaID"),
                    UsageCode = dr.GetString("UsageCode"),
                    UsageCodeDescription = dr.GetString("Description"),
                    SubArdCode = dr.GetString("SubArdCode"),
                    Amount = dr.GetDecimal("Amount"),
                    Reference = dr.GetString("Reference"),
                    CostCenterVal = dr.GetString("CostCenter"),
                    DimensionVal = dr.GetString("Dimension"),
                    CostCenter = dr.GetInt32("CostCenterID"),
                    Dimension = dr.GetInt32("DimensionID")
                };
            }
        }

        // Method to select a sundry payment entry by its ID
        public SundryPaymentModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentMemoID", ID),
               new SqlParameter("@Operation", "GetEntries")
            };
            SundryPaymentModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryPayment.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to approve a sundry payment entry
        public int Approve(int? SundryPaymentID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SundryPaymentID", SundryPaymentID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryPayment.Approve]", parameters);
        }

        // Method to reverse a sundry payment entry
        public int Reverse(int? SundryPaymentID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SundryPaymentID", SundryPaymentID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryPayment.Reverse]", parameters);
        }

        // Method to update a sundry payment entry
        public SundryPaymentModel Update(SundryPaymentModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SundryPaymentID", model.SundryPaymentID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@PaymentMode", model.PaymentMode),
               new SqlParameter("@PaymentArdCode", model.PaymentArdCode),
               new SqlParameter("@BankCharges", model.BankCharges),
               new SqlParameter("@PaymentTool", model.PaymentTool),
               new SqlParameter("@PayeeName", model.PayeeName),
               new SqlParameter("@PayeeContactNo", model.PayeeContactNo),
               new SqlParameter("@CoaID", model.PaymentCoaID),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@entries", model.Entries.ToDataTable()),
               new SqlParameter("@deferredExpenditure", model.deferredExpenditure),
               new SqlParameter("@beginDate", model.beginDate),
               new SqlParameter("@EndDate", model.EndDate),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@IsInterBranch", model.IsInterBranch),
               new SqlParameter("@InterBranchTranID", model.InterBranchTranID),
               new SqlParameter("@OtherBranchID", model.OtherBranchID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@JobcardID", model.JobcardID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryPayment.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to SundryPaymentModel
        private SundryPaymentModel GetModel(IDataRecord dr)
        {
            return new SundryPaymentModel
            {
                SundryPaymentID = dr.GetInt32("SundryPaymentID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                PaymentMode = dr.GetString("PaymentMode"),
                PaymentArdCode = dr.GetString("ArdCode"),
                PaymentTool = dr.GetString("PaymentTool"),
                PayeeName = dr.GetString("PayeeName"),
                PayeeContactNo = dr.GetString("PayeeContactNo"),
                PaymentCoaID = dr.GetInt32("CoaID"),
                AccountName = dr.GetString("AccountName"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                Reference = dr.GetString("Reference"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocNumber"),
                MID = dr.GetInt32("MID"),
                FileName = dr.GetString("FilePath"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                BankCharges = dr.GetDecimal("BankCharges"),
                deferredExpenditure = dr.GetBoolean("deferredExpenditure"),
                beginDate = dr.GetDateTime("beginDate"),
                EndDate = dr.GetDateTime("EndDate"),
                Narration = dr.GetString("Narration"),
                IsInterBranch = dr.GetBoolean("IsInterBranch"),
                InterBranchTranID = dr.GetInt32("InterBranchTranID"),
                OtherBranchID = dr.GetInt32("OtherBranchID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public IEnumerable<SundryPaymentModel> Select(int? BranchID)
        {
            throw new NotImplementedException();
        }
    }
}