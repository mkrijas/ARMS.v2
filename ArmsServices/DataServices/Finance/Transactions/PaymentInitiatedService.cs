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
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Initiate]", parameters))
            {
                model.PaymentInitiatedID = dr.GetInt32("PaymentInitiatedID");
            }
            return model.PaymentInitiatedID;
        }

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

        public int? Reverse(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.PaymentMemo.Initiate.Reverse]", parameters);

        }

        public int? Approve(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PIID", ID),
               new SqlParameter("@Narration", Remarks),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.PaymentMemo.Initiate.Approve]", parameters);
            
        }

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
    }
}
