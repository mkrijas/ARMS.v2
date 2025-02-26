using ArmsModels.BaseModels;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using System.Data;

namespace ArmsServices.DataServices
{
    public class PaymentFinalizeService : IPaymentFinalizeService
    {
        IDbService Iservice;
        public PaymentFinalizeService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to update a payment finalize entry 
        public int? Update(PaymentFinishModel model)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Reference");
            dt.Columns.Add("Amount");
            foreach (var item in model.SelectedMemos) {
            if(item.BankCharges!= null && item.BankCharges > 0)
                {
                    DataRow row = dt.NewRow();
                    row["ID"] = item.PaymentMemoID;
                    row["Reference"] = item.Reference;
                    row["Amount"] = item.BankCharges;
                    dt.Rows.Add(row);
                }
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentInitiatedID", model.PaymentInitiatedID),
               new SqlParameter("@PaymentFinalizeID", model.PaymentFinalizeID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@CoaID", model.PaymentCoaID),
               new SqlParameter("@PaymentArdCode", model.PaymentArdCode),
               new SqlParameter("@PaymentMode", model.PaymentMode),
               new SqlParameter("@PaymentTool", model.PaymentTool),
               new SqlParameter("BankCharges", dt),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Finish]", parameters))
            {
                model.PaymentFinalizeID = dr.GetInt32("PFID");
            }
            return model.PaymentFinalizeID;
        }

        // Method to select payment finalize entries based on various parameters
        public IEnumerable<PaymentFinishModel> Select(int? BranchID, int? PfID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@ID", PfID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Finish.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select payment finalize entries by date period
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

        // Method to approve a payment finalize entry
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

        // Helper method to convert IDataRecord to PaymentFinishModel
        PaymentFinishModel GetModel(IDataRecord dr)
        {
            return new PaymentFinishModel()
            {                
                BranchID = dr.GetInt32("BranchID"),                
                PaymentFinalizeID = dr.GetInt32("PaymentFinalizeID"),
                PaymentInitiatedID = dr.GetInt32("PaymentInitiatedID"),
                DueOn = dr.GetDateTime("DueOn"), 
                DocumentNumber = dr.GetString("DocumentNumber"),
                EffectiveDate = dr.GetDateTime("EffectiveDate"),
                InitiatedDocumentDate = dr.GetDateTime("InitiatedDocumentDate"), 
                MID = dr.GetInt32("MID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),                
                FileName = dr.GetString("FilePath"),                
                OtherBranchID = dr.GetInt32("OtherBranchID"),
                IsInterBranch = dr.GetBoolean(""),
                InterBranchTranID = dr.GetInt32("InterBranchTranID"),
                PaymentArdCode = dr.GetString("ArdCode"),
                PaymentCoaID = dr.GetInt32("CoaID"),                
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