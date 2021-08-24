using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITransactionService
    {
        OperationTransactionModel Update(OperationTransactionModel model);
        int Delete(long ID, string UserID);
        IEnumerable<OperationTransactionModel> SelectByTrip(long? TripID);
        OperationTransactionModel SelectByID(long? ID);
    }

    public class TransactionService : ITransactionService
    {
        IDbService Iservice;

        public TransactionService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public OperationTransactionModel Update(OperationTransactionModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RefType", model.RefType),
               new SqlParameter("@RefID", model.RefID),
               new SqlParameter("@FinanceDocID", model.FinanceDocID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@TransactionDate", model.TransactionDate),
               new SqlParameter("@TransactionID", model.TransactionID),
               new SqlParameter("@Transactions", model.Transactions.ToDataTable()),                  
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (var dr in Iservice.GetDataReader("[usp.Operation.Trip.Transaction.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        public int Delete(long ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Operation.Transaction.Delete]", parameters);
        }
        public OperationTransactionModel SelectByID(long? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID)
            };

            OperationTransactionModel model = null;
            foreach (var dr in Iservice.GetDataReader("[usp.Operation.Transaction.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<OperationTransactionModel> SelectByTrip(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID)
            };            
            foreach (var dr in Iservice.GetDataReader("[usp.Operation.Transaction.Select]", parameters))
            {
                yield return GetModel(dr);
            }           
        }

        private IEnumerable<OpTranSubModel> GetChildren(long TransactionID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TransactionID", TransactionID)
            };
            foreach (var dr in Iservice.GetDataReader("[usp.Operation.Transaction.Sub.Select]", parameters))
            {
                decimal SignedAmount = dr.GetDecimal("Amount");
                yield return new()
                {
                    TariffID = dr.GetInt16("TariffID"),
                    TransactionID = dr.GetInt64("TransactionID"),
                    TransactionSubID = dr.GetInt64("TransactionSubID"),
                    Sign = Math.Sign(SignedAmount),
                    Amount = Math.Abs(SignedAmount),
                    BillDate = dr.GetDateTime("BillDate"),
                    FinanceTranID = dr.GetInt64("FinanceTranID"),
                    Quantity = dr.GetDecimal("Quantity"),
                    Reference = dr.GetString("Reference"),
                };
            }
        }

        private OperationTransactionModel GetModel(IDataRecord dr)
        {
            return new OperationTransactionModel
            {               
               TransactionDate = dr.GetDateTime("TransactionDate"),
               TransactionID = dr.GetInt64("TransactionID"),               
               RefID = dr.GetInt64("RefID"),
               RefType = dr.GetString("RefType"),
               BranchID = dr.GetInt32("BranchID"),
               FinanceDocID = dr.GetInt64("FinanceDocID"),
               Transactions = GetChildren(dr.GetInt64("TransactionID")).ToList(),
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
