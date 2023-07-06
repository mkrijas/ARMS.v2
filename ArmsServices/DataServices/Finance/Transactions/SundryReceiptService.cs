using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels.Finance.Transactions;

namespace ArmsServices.DataServices
{
    public class SundryReceiptService : ISundryReceiptService
    {
        IDbService Iservice;

        public SundryReceiptService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SundryReceiptID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryReceipt.Delete]", parameters);

        }

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

        public IEnumerable<SundryReceiptModel> SelectByApproved(int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<SundryReceiptModel> SelectByUnapproved(int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

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
                    Amount = dr.GetDecimal("Amount"),
                    Rederence = dr.GetString("Reference")
                };
            }
        }

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

        public int Reverse(int? SundryReceiptID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SundryReceiptID", SundryReceiptID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryReceipt.Reverse]", parameters);
        }
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
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@entries", model.Entries.ToDataTable()),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryReceipt.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
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
