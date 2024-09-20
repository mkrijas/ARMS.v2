using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class CreditDebitTransferService : ICreditDebitTransferService
    {
        IDbService Iservice;
        public CreditDebitTransferService(IDbService iservice)
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

        public IEnumerable<CreditDebitTransferModel> Select(int? BranchID)
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

        public IEnumerable<CreditDebitTransferModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
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

        public IEnumerable<CreditDebitTransferModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
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

        public CreditDebitTransferModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ReceiptID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            CreditDebitTransferModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.CreditTransfer.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
  

        public IEnumerable<CreditDebitTransferModel> SelectByPeriod(DateTime? begin, DateTime? end, int? BranchID)
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

        public CreditDebitTransferModel Update(CreditDebitTransferModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@TransactionMode", model.TransactionMode),
               new SqlParameter("@OriginPartyType", model.OriginPartyType),
               new SqlParameter("@TargetPartyType", model.TargetPartyType),
               new SqlParameter("@OriginSubArdCode", model.OriginSubArdCode.SubArdCode),
               new SqlParameter("@TargetSubArdCode", model.TargetSubArdCode.SubArdCode),
               new SqlParameter("@OriginPartyID", model.OriginParty.PartyID),
               new SqlParameter("@TargetPartyID", model.TargetParty.PartyID),
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
        private CreditDebitTransferModel GetModel(IDataRecord dr)
        {
            return new CreditDebitTransferModel
            {
                ID = dr.GetInt32("ID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                BranchID = dr.GetInt32("BranchID"),
                BranchName = dr.GetString("BranchName"),
                OtherBranchID = dr.GetInt32("OtherBranch"),
                OtherBranchName = dr.GetString("OtherBranchName"),
                InterBranchTranID = dr.GetInt32("InterBranchTranID"),
                IsInterBranch = dr.GetBoolean("IsInterBranch"),
                TransactionMode = dr.GetInt32("TransactionMode"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                OriginPartyType = dr.GetString("OriginPartyType"),
                OriginCoaID = dr.GetInt32("OriginPartyCoa"),
                OriginParty = new PartyModel() 
                    { 
                      PartyID = dr.GetInt32("OriginPartyID"),
                      PartyCode = dr.GetString("OriginPartyCode"),
                      TradeName = dr.GetString("OriginPartyName") 
                    },
                OriginSubArdCode = new SubArdCodeModel()
                {
                    SubArdCode = dr.GetString("OriginSubArdCode"),
                    TranType = dr.GetString("OriginTranType"),
                },
                TargetPartyType= dr.GetString("TargetPartyType"),
                TargetCoaID = dr.GetInt32("TargetPartyCoa"),                
                TargetParty = new PartyModel()  
                    { 
                      PartyID = dr.GetInt32("TargetPartyID"),
                      PartyCode = dr.GetString("TargetPartyCode"),
                      TradeName = dr.GetString("TargetPartyName") 
                    },
                TargetSubArdCode = new SubArdCodeModel()
                {
                    SubArdCode = dr.GetString("TargetSubArdCode"),
                    TranType = dr.GetString("TargetTranType"),
                },
                Narration = dr.GetString("Narration"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                FileName = dr.GetString("FilePath"),                
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),                
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