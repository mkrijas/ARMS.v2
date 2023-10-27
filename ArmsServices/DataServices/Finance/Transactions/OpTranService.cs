using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

using System.Diagnostics;


namespace ArmsServices.DataServices
{
    public class OpTranService : IOpTranService
    {
        IDbService Iservice;
        public OpTranService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public OpTranModel Update(OpTranModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OpTranID", model.OpTranID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@TripID", model.TripID),
               new SqlParameter("@AssetTransferID", model.AssetTransferID),
               new SqlParameter("@CreditCoaID", model.CreditCoaID),
               new SqlParameter("@Area", model.Area),
               new SqlParameter("@JobCardID", model.JobCardID),
               new SqlParameter("@PaymentArdCode", model.PaymentArdCode),
               new SqlParameter("@PaymentMode", model.PaymentMode),
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@MID", model.MID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@Expenses", model.Transactions.ToDataTable()),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (var dr in Iservice.GetDataReader("[usp.Finance.Transactions.OpTran.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public int Delete(long? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.OpTran.Delete]", parameters);
        }
        public OpTranModel SelectByID(long? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID"),
            };

            OpTranModel model = null;
            foreach (var dr in Iservice.GetDataReader("[usp.Finance.Transactions.OpTran.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<OpTranModel> SelectByTrip(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Operation", "ByTrip"),
            };

            foreach (var dr in Iservice.GetDataReader("[usp.Finance.Transactions.OpTran.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<OpTranModel> SelectByApprovedTrip(int? BranchID, long? TripID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (var dr in Iservice.GetDataReader("[usp.Finance.Transactions.OpTran.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<OpTranModel> SelectByUnapprovedTrip(int? BranchID, long? TripID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (var dr in Iservice.GetDataReader("[usp.Finance.Transactions.OpTran.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<OpTranModel> SelectByJobcard(int? JobcardID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JobcardID", JobcardID),
               new SqlParameter("@Operation", "ByJobCard"),
            };

            foreach (var dr in Iservice.GetDataReader("[usp.Finance.Transactions.OpTran.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<OpTranSubModel> GetExpenses(long? TransactionID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OpTranID", TransactionID),
               new SqlParameter("@Operation", "GetExpenses"),
            };
            foreach (var dr in Iservice.GetDataReader("[usp.Finance.Transactions.OpTran.Select]", parameters))
            {
                yield return new OpTranSubModel()
                {                  
                    ExpenseUsageCode = dr.GetString("ExpenseUsageCode"),  
                    CoaID = dr.GetInt32("CoaID"),
                    OpTranID = dr.GetInt32("OpTranID"),                    
                    OpTranSubID = dr.GetInt64("OpTranSubID"),
                    Amount = dr.GetDecimal("Amount"),
                    Quantity = dr.GetDecimal("Quantity"),
                    Reference = dr.GetString("Reference"),
                    Unit = dr.GetString("Unit")
                };
            }
        }

        public int Approve(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.OpTran.Approve]", parameters);
        }
        public int Reverse(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.OpTran.Reverse]", parameters);
        }

        private OpTranModel GetModel(IDataRecord dr)
        {
            return new OpTranModel
            {
                DocumentDate = dr.GetDateTime("DocumentDate"),
                Dimension = dr.GetInt32("Dimension"),
                CostCenter = dr.GetInt32("CostCenter"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),                
                Area = dr.GetString("Area"),
                JobCardID = dr.GetInt32("JobCardID"),
                PaymentArdCode = dr.GetString("PaymentArdCode"),
                PaymentMode = dr.GetString("PaymentMode"),
                TruckID = dr.GetInt32("TruckID"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                BranchID = dr.GetInt32("BranchID"),
                FileName = dr.GetString("FilePath"),
                CreditCoaID = dr.GetInt32("CreditCoaID"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                OpTranID = dr.GetInt32("OpTranID"),
                Narration = dr.GetString("Narration"),
                MID = dr.GetInt32("MID"),
                TripID = dr.GetInt64("TripID"),
                AssetTransferID = dr.GetInt32("AssetTransferID"),
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
