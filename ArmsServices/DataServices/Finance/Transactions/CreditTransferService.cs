using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class CreditTransferService : ICreditTransferService
    {
        IDbService Iservice;
        public CreditTransferService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public int Approve(int? ID, string UserID, string remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.CreditTransfer.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.CreditTransfer.Delete]", parameters);
        }

        public IEnumerable<CreditTransferModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.CreditTransfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<CreditTransferModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", IsInterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.CreditTransfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<CreditTransferModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", IsInterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.CreditTransfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }       

        public CreditTransferModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ReceiptID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            CreditTransferModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.CreditTransfer.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
  

        public IEnumerable<CreditTransferModel> SelectByPeriod(DateTime? begin, DateTime? end, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.CreditTransfer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public CreditTransferModel Update(CreditTransferModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@TransactionMode", model.TransactionMode),
               new SqlParameter("@OriginPartyType", model.OriginPartyType),
               new SqlParameter("@TargetPartyType", model.TargetPartyType),
               new SqlParameter("@OriginSubArdCode", model.OriginSubArdCode),
               new SqlParameter("@TargetSubArdCode", model.TargetSubArdCode),
               new SqlParameter("@OriginPartyID", model.OriginPartyID),
               new SqlParameter("@TargetPartyID", model.TargetPartyID),
               new SqlParameter("@OriginCoaID", model.OriginCoaID),
               new SqlParameter("@TargetCoaID", model.TargetCoaID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@OtherBranch", model.OtherBranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),               
               new SqlParameter("@RecordStatus", model.UserInfo.RecordStatus),               
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@TimeStamp", model.UserInfo.TimeStampField),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@IsInterBranch", model.IsInterBranch),
               new SqlParameter("@InterBranchTranID", model.InterBranchTranID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.CreditTransfer.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        private CreditTransferModel GetModel(IDataRecord dr)
        {
            return new CreditTransferModel
            {
                ID = dr.GetInt32("ID"),
                TransactionMode = dr.GetInt32("TransactionMode"),
                OriginPartyID = dr.GetInt32("OriginPartyID"),
                TargetPartyID = dr.GetInt32("TargetPartyID"),
                OriginPartyType = dr.GetString("OriginPartyType"),
                OriginSubArdCode = dr.GetString("OriginSubArdCode"),
                OriginCoaID = dr.GetInt32("OriginCoaID"),
                TargetSubArdCode = dr.GetString("TargetSubArdCode"),
                TargetPartyType= dr.GetString("TargetPartyType"),
                TargetCoaID = dr.GetInt32("TargetCoaID"),                
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),                
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
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.CreditTransfer.Reverse]", parameters);
        }

        public int RemoveFile(int? ID, string UserID)
        {
            throw new NotImplementedException();
        }
    }
}