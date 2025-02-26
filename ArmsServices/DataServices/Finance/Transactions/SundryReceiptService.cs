using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class SundryReceiptService : ISundryReceiptService
    {
        IDbService Iservice;

        public SundryReceiptService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a sundry receipt entry
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "DELETE"),
               new SqlParameter("@SundryReceiptID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryReceipt.Delete]", parameters);
        }

        // Method to remove a file associated with a sundry receipt entry
        public int RemoveFile(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "REMOVEFILE"),
               new SqlParameter("@SundryReceiptID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryReceipt.Delete]", parameters);
        }

        // Method to select all sundry receipt entries
        public IEnumerable<SundryReceiptModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select approved sundry receipt entries
        public IEnumerable<SundryReceiptModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", IsInterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select unapproved sundry receipt entries
        public IEnumerable<SundryReceiptModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", IsInterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to get entries associated with a specific sundry receipt
        public IEnumerable<SundryReceiptEntryModel> GetEntries(int? SID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetEntries"),
               new SqlParameter("@SundryReceiptID", SID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryReceipt.Select]", parameters))
            {
                yield return new SundryReceiptEntryModel()
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

        // Method to select a sundry receipt entry by its ID
        public SundryReceiptModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SundryReceiptID", ID),
               new SqlParameter("@Operation", "GetEntries")
            };
            SundryReceiptModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryReceipt.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to approve a sundry receipt entry
        public int Approve(int? SundryReceiptID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SundryReceiptID", SundryReceiptID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryReceipt.Approve]", parameters);
        }

        // Method to update a sundry receipt entry
        public SundryReceiptModel Update(SundryReceiptModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SundryReceiptID", model.SundryReceiptID),
               new SqlParameter("@ReceiptMode", model.ReceiptMode),
               new SqlParameter("@ArdCode", model.ReceiptArdCode),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@PayerName", model.PayerName),
               new SqlParameter("@PayerContactNo", model.PayerContactNo),
               new SqlParameter("@CoaID", model.ReceiptCoaID),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@entries", model.Entries.ToDataTable()),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@IsInterBranch", model.IsInterBranch),
               new SqlParameter("@InterBranchTranID", model.InterBranchTranID),
               new SqlParameter("@OtherBranchID", model.OtherBranchID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryReceipt.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to SundryReceiptModel
        private SundryReceiptModel GetModel(IDataRecord dr)
        {
            return new SundryReceiptModel
            {
                SundryReceiptID = dr.GetInt32("SundryReceiptID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                ReceiptMode = dr.GetString("ReceiptMode"),
                ReceiptArdCode = dr.GetString("ArdCode"),
                PayerName = dr.GetString("PayerName"),
                PayerContactNo = dr.GetString("PayerContactNo"),
                ReceiptCoaID = dr.GetInt32("CoaID"),
                Reference = dr.GetString("Reference"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocNumber"),
                MID = dr.GetInt32("MID"),
                FileName = dr.GetString("FilePath"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
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

        public int Reverse(int? PID, string UserID, string Remarks)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SundryReceiptModel> Select(int? BranchID)
        {
            throw new NotImplementedException();
        }
    }
}