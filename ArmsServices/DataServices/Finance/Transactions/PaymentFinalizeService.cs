using ArmsModels.BaseModels;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using System.Data;

namespace ArmsServices.DataServices
{
    public interface IPaymentFinalizeService
    {
        int Approve(int? PID, string UserID, string Remarks);
        IEnumerable<PaymentFinishModel> Select(int? BranchID, int? FinalizeID);
        IEnumerable<PaymentFinishModel> SelectByPeriod(int? BranchID, DateTime Begin, DateTime End);
        int? Update(PaymentFinishModel model);
    }
    public class PaymentFinalizeService : IPaymentFinalizeService
    {
        IDbService Iservice;
        public PaymentFinalizeService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public int? Update(PaymentFinishModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentInitiatedID", model.PaymentInitiatedID),
               new SqlParameter("@PaymentFinalizeID", model.PaymentFinalizeID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@BankCharges", model.BankCharges),
               new SqlParameter("@CoaID", model.PaymentCoaID),
               new SqlParameter("@PaymentArdCode", model.PaymentArdCode),
               new SqlParameter("@PaymentMode", model.PaymentMode),
               new SqlParameter("@PaymentTool", model.PaymentTool),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Finish]", parameters))
            {
                model.PaymentFinalizeID = dr.GetInt32("PcID");
            }
            return model.PaymentFinalizeID;
        }

        public IEnumerable<PaymentFinishModel> Select(int? BranchID, int? PfID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
                 new SqlParameter("@PfID", PfID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Finish.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<PaymentFinishModel> SelectByPeriod(int? BranchID, DateTime Begin, DateTime End)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", Begin),
               new SqlParameter("@end", End),
               new SqlParameter("@BranchID", BranchID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Finish.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public int Approve(int? PfID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@pfID", PfID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.PaymentMemo.Finish.Approve]", parameters);
        }



        PaymentFinishModel GetModel(IDataRecord dr)
        {
            return new PaymentFinishModel()
            {
                BranchID = dr.GetInt32("BranchID"),
                PaymentMemoID = dr.GetInt32("PaymentMemoID"),
                PaymentFinalizeID = dr.GetInt32("PaymentFinalizeID"),
                PaymentInitiatedID = dr.GetInt32("PaymentInitiatedID"),
                DueOn = dr.GetDateTime("DueOn"),
                PaymentDocumentNumber = dr.GetString("PaymentDocumentNumber"),
                PaymentStatus = dr.GetByte("PaymentStatus"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                EffectiveDate = dr.GetDateTime("EffectiveDate"),
                InitiatedDocumentDate = dr.GetDateTime("InitiatedDocumentDate"),
                PaymentDocumentDate = dr.GetDateTime("PaymentDocumentDate"),
                PartyInfo = new PartyModel() { PartyID = dr.GetInt32("PartyID"), TradeName = dr.GetString("TradeName") },
                MID = dr.GetInt32("MID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                CostCenter = dr.GetInt32("CostCenter"),
                Dimension = dr.GetInt32("Dimension"),
                PaymentArdCode = dr.GetString("PaymentArdCode"),
                PaymentCoaID = dr.GetInt32("CoaID"),
                BankCharges = dr.GetDecimal("BankCharges"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                Narration = dr.GetString("Narration"),
                PaymentMode = dr.GetString("PaymentMode"),
                PaymentTool = dr.GetString("PaymentTool"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
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

    }
}
