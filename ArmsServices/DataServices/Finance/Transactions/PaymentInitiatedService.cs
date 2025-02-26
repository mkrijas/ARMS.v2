using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System;
using System.Reflection;

namespace ArmsServices.DataServices
{
    public class PaymentInitiatedService : IPaymentInitiatedService
    {
        IDbService Iservice;
        public PaymentInitiatedService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to update an initiated payment entry
        public int? Update(PaymentInitiatedModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentInitiatedID", model.PaymentInitiatedID),
               new SqlParameter("@Memos", model.SelectedMemos.Select(x => x.PaymentMemoID.Value).ToList().ToDataTable()),
               new SqlParameter("@InitiatedDocumentDate", model.InitiatedDocumentDate),
               new SqlParameter("@DueOn", model.DueOn),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@IsInterBranch", model.IsInterBranch),
               new SqlParameter("@InterBranchTranID", model.InterBranchTranID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Initiate]", parameters))
            {
                model.PaymentInitiatedID = dr.GetInt32("PaymentInitiatedID");
            }
            return model.PaymentInitiatedID;
        }

        // Method to retrieve pending payment entries for completion
        public IEnumerable<PaymentInitiatedModel> PendingForCompletion(int? BranchID, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ToComplete"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@searchTerm", searchTerm)

            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Initiate.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select initiated payments by branch ID
        public IEnumerable<PaymentInitiatedModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Initiate.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select initiated payments between specified dates
        public IEnumerable<PaymentInitiatedModel> SelectInitiatedBetween(int? BranchID, DateTime Begin, DateTime End)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", Begin),
               new SqlParameter("@end", End),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Initiate.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Helper method to convert IDataRecord to PaymentInitiatedModel
        PaymentInitiatedModel GetModel(IDataRecord dr)
        {
            return new PaymentInitiatedModel()
            {
                BranchID = dr.GetInt32("BranchID"),
                DueOn = dr.GetDateTime("DueOn"),
                PaymentInitiatedID = dr.GetInt32("PaymentInitiatedID"),                
                DocumentNumber = dr.GetString("InitiatedDocumentNumber"),
                InitiatedDocumentDate = dr.GetDateTime("InitiatedDocumentDate"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                IsInterBranch = dr.GetBoolean("IsInterBranch"),
                InterBranchTranID = dr.GetInt32("InterBranchTranID"),
                AuthLevelId = dr.GetInt32("AuthLevelID"),
                AuthStatus = dr.GetString("AuthStatus"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Method to reverse an initiated payment entry
        public int? Reverse(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.PaymentMemo.Initiate.Reverse]", parameters);

        }

        // Method to approve an initiated payment entry
        public int? Approve(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PIID", ID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.PaymentMemo.Initiate.Approve]", parameters);
            
        }

        // Method to get payment memo print details
        public IEnumerable<PaymentMemoPrintDetailModel> GetPaymentMemoPrintDetails(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetMemoPrintDetails"),               
               new SqlParameter("@PaymentInitiatedID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Initiate.Select]", parameters))
            {
                yield return new PaymentMemoPrintDetailModel()
                {
                    Amount = dr.GetDecimal("TotalAmount"),
                    BankAccount = dr.GetString("AccountNumber"),
                    BeneficiaryName = dr.GetString("BeneficiaryName"),
                    IfscCode = dr.GetString("IfscCode"),
                    PartyName = dr.GetString("tradeName"),
                    PaymentMemoID = dr.GetInt32("PaymentMemoID"),
                    DocNumber = dr.GetString("DocNumber"),
                    DueOn = dr.GetDateTime("DueOn")
                };
            }
        }

        // Method to delete a payment memo entry
        public int? Delete(int? PaymentMemoID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentMemoID", PaymentMemoID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.PaymentMemo.Initiate.Delete]", parameters);

        }

    }
}
