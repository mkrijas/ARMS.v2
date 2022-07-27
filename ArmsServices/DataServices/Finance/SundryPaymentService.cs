using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels.Finance.Transactions;

namespace ArmsServices.DataServices
{
    public interface ISundryPaymentService
    {
        SundryPaymentModel Update(SundryPaymentModel model);
        SundryPaymentModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<SundryPaymentModel> Select();
        IEnumerable<SundryPaymentEntryModel> GetEntries(int? SID);
        int Approve(int? SundryPaymentID, string UserID);
    }
    public class SundryPaymentService : ISundryPaymentService
    {
        IDbService Iservice;

        public SundryPaymentService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentMemoID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.PaymentMemo.Delete]", parameters);

        }

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
                    AccountName = dr.GetString("AccountName"),
                    Amount = dr.GetDecimal("Amount"),
                    Rederence = dr.GetString("Reference")
                   
                };
            }
        }

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
        public int Approve(int? SundryPaymentID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SundryPaymentID", SundryPaymentID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Status", 1)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryPayment.Approve]", parameters);
        }

        public SundryPaymentModel Update(SundryPaymentModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SundryPaymentID", model.SundryPaymentID),
               new SqlParameter("@PaymentMode", model.PaymentMode),
               new SqlParameter("@CoaID", model.CoaID),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@entries", model.Entries.ToDataTable()),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),   
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryPayment.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        private SundryPaymentModel GetModel(IDataRecord dr)
        {
            return new SundryPaymentModel
            {
                SundryPaymentID = dr.GetInt32("SundryPaymentID"),
                PaymentMode = dr.GetString("PaymentMode"),
                CoaID = dr.GetInt32("CoaID"),
                AccountName=dr.GetString("AccountName"),
                Reference=dr.GetString("Reference"),
                BranchID = dr.GetInt32("BranchID"),
                // BranchName = dr.GetString("BranchName"),
                ApprovedInfo = new ArmsModels.SharedModels.UserInfoModel()
                {
                    RecordStatus = dr.GetByte("ApprovedStatus"),
                    TimeStampField = dr.GetDateTime("ApprovedOn"),
                    UserID = dr.GetString("ApprovedBy"),
                },
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                CostCenter = dr.GetInt32("CostCenter"),

                Dimension = dr.GetInt32("Dimension"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
               
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
