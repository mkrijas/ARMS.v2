using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace ArmsServices.DataServices
{
    public interface IPaymentInitiatedService
    {
        IEnumerable<PaymentInitiatedModel> PendingForCompletion(int? BranchID);
        int? Update(PaymentInitiatedModel model);
        IEnumerable<PaymentInitiatedModel> Select(int? BranchID);        
        IEnumerable<PaymentInitiatedModel> SelectInitiatedBetween(int? BranchID, DateTime Begin, DateTime End);
    }

    public class PaymentInitiatedService: IPaymentInitiatedService
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
               new SqlParameter("@PaymentMemoID", model.PaymentMemoID),
               new SqlParameter("@InitiatedDocumentDate", model.InitiatedDocumentDate),
               new SqlParameter("@DueOn", model.DueOn),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Initiate]", parameters))
            {
                model.PaymentInitiatedID = dr.GetInt32("PiID");
            }
            return model.PaymentInitiatedID;
        }

        public IEnumerable<PaymentInitiatedModel> PendingForCompletion(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ToComplete"),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return new PaymentInitiatedModel()
                {
                    BranchID = dr.GetInt32("BranchID"),
                    DueOn = dr.GetDateTime("DueOn"),
                    DocumentNumber = dr.GetString("DocNumber"),
                    DocumentDate = dr.GetDateTime("DocDate"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
                    PaymentInitiatedID = dr.GetInt32("PiID"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
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
                yield return new PaymentInitiatedModel()
                {
                    BranchID = dr.GetInt32("BranchID"),
                    DueOn = dr.GetDateTime("DueOn"),
                    PaymentInitiatedID = dr.GetInt32("PiID"),
                    DocumentNumber = dr.GetString("DocNumber"),
                    AuthLevelId = dr.GetInt32("AuthLevelId"),
                    AuthStatus = dr.GetString("AuthStatus"),
                    DocumentDate = dr.GetDateTime("DocDate"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
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
                yield return new PaymentInitiatedModel()
                {
                    BranchID = dr.GetInt32("BranchID"),
                    DueOn = dr.GetDateTime("DueOn"),
                    PaymentInitiatedID = dr.GetInt32("PiID"),
                    DocumentNumber = dr.GetString("DocNumber"),
                    DocumentDate = dr.GetDateTime("DocDate"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
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
}
