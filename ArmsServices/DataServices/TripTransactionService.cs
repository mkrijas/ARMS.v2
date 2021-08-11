using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITripTransactionService
    {
        TripTransactionModel Update(TripTransactionModel model);
        int Delete(long ID, string UserID);
        IEnumerable<TripTransactionModel> SelectByTrip(long? TripID);
        TripTransactionModel SelectByID(long? ID);
        IEnumerable<TripTransactionTypeModel> GetTypes();
        TripTransactionTypeModel UpdateType(TripTransactionTypeModel model);
    }

    public class TripTransactionService : ITripTransactionService
    {
        IDbService Iservice;

        public TripTransactionService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public TripTransactionModel Update(TripTransactionModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", model.TripID),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@BillDate", model.BillDate),
               new SqlParameter("@FinanceTranID", model.FinanceTranID),
               new SqlParameter("@Quantity", model.Quantity),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@TransactionDate", model.TransactionDate),
               new SqlParameter("@TransactionID", model.TransactionID),
               new SqlParameter("@TransactionTypeID", model.TransactionTypeID),               
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
            return Iservice.ExecuteNonQuery("[usp.Operation.Trip.Delete]", parameters);
        }
        public TripTransactionModel SelectByID(long? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID)
            };

            TripTransactionModel model = null;
            foreach (var dr in Iservice.GetDataReader("[usp.Operation.Trip.Transaction.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<TripTransactionModel> SelectByTrip(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID)
            };            
            foreach (var dr in Iservice.GetDataReader("[usp.Operation.Trip.Transaction.Select]", parameters))
            {
                yield return GetModel(dr);
            }
           
        }

        private TripTransactionModel GetModel(IDataRecord dr)
        {
            return new TripTransactionModel
            {
               Amount = dr.GetDecimal("Amount"),
               BillDate = dr.GetDateTime("BillDate"),
               FinanceTranID = dr.GetInt64("FinanceTranID"),
               Quantity = dr.GetDecimal("Quantity"),
               Reference = dr.GetString("Reference"),
               TransactionDate = dr.GetDateTime("TransactionDate"),
               TransactionID = dr.GetInt64("TransactionID"),
               TransactionTypeID=dr.GetInt32("TransactionTypeID"),
               TripID=dr.GetInt64("TripID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public IEnumerable<TripTransactionTypeModel> GetTypes()
        {            
            foreach (var dr in Iservice.GetDataReader("[usp.Operation.Trip.TransactionType.Select]", null))
            {
                yield return GetTypeModel(dr);
            }
        }


        private TripTransactionTypeModel GetTypeModel(IDataRecord dr)
        {
            return new TripTransactionTypeModel()
            {
                AllowMultiple = dr.GetBoolean("AllowMultiple"),
                CrDr = dr.GetString("CrDr"),
                FinancialAccountID = dr.GetInt32("FinancialAccountID"),
                Nature = dr.GetString("Nature"),
                TransactionTypeID = dr.GetInt32("TransactionTypeID"),
                TTName = dr.GetString("TTName"),
                Unit = dr.GetString("Unit"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public TripTransactionTypeModel UpdateType(TripTransactionTypeModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AllowMultiple", model.AllowMultiple),
               new SqlParameter("@CrDr", model.CrDr),
               new SqlParameter("@FinancialAccountID", model.FinancialAccountID),
               new SqlParameter("@Nature", model.Nature),
               new SqlParameter("@TransactionTypeID", model.TransactionTypeID),
               new SqlParameter("@TTName", model.TTName),
               new SqlParameter("@Unit", model.Unit),               
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (var dr in Iservice.GetDataReader("[usp.Operation.Trip.TransactionType.Update]", parameters))
            {
                model = GetTypeModel(dr);
            }
            return model;
        }
    }
}
