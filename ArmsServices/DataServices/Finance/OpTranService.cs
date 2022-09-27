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
    public interface IOpTranService
    {
        OpTranModel Update(OpTranModel model);
        int Delete(long? ID, string UserID);
        IEnumerable<OpTranModel> SelectByTrip(long? TripID);        
        OpTranModel SelectByID(long? ID);
        int Approve(int? ID, string UserID);
        int Reverse(int? ID, string UserID);

    }

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
               new SqlParameter("@TripID", model.TripID),
               new SqlParameter("@CreditCoaID", model.CreditCoaID),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@MID", model.MID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
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
       

        private IEnumerable<OpTranSubModel> GetExpenses(long? TransactionID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", TransactionID),
               new SqlParameter("@Operation", "GetExpenses"),
            };
            foreach (var dr in Iservice.GetDataReader("[usp.Finance.Transactions.OpTran.Select]", parameters))
            {    
                yield return new OpTranSubModel()
                {
                    ExpenseID  = dr.GetInt32("ExpenseID"),                     
                    OpTranID = dr.GetInt32("OpTranID"),
                    OpTranSubID = dr.GetInt64("OpTranSubID"),                    
                    Amount = dr.GetDecimal("Amount"),             
                    Quantity = dr.GetDecimal("Quantity"),
                    Reference = dr.GetString("Reference"),
                    Unit = dr.GetString("Unit")
                }; 
            }
        }

        public int Approve(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Status", 1)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.OpTran.Approve]", parameters);
        }
        public int Reverse(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Status", 2)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.OpTran.Approve]", parameters);
        }

        private OpTranModel GetModel(IDataRecord dr)
        {
            return new OpTranModel
            {               
               DocumentDate = dr.GetDateTime("DocumentDate"),
               Dimension = dr.GetInt32("Dimension"),               
               CostCenter = dr.GetInt32("CostCenter"),
               TotalAmount = dr.GetDecimal("TotalAmount"),
               BranchID = dr.GetInt32("BranchID"),
               CreditCoaID = dr.GetInt32("CreditCoaID"), 
               DocumentNumber = dr.GetString("DocumentNumber"),
               OpTranID = dr.GetInt32("OpTranID"),
               Narration = dr.GetString("Narration"),
               MID = dr.GetInt32("MID"),
               TripID = dr.GetInt64("TripID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
                ApprovedInfo =  new ArmsModels.SharedModels.UserInfoModel()
                {
                    RecordStatus = dr.GetByte("ApprovedStatus"),
                    TimeStampField = dr.GetDateTime("ApprovedOn"),
                    UserID = dr.GetString("ApprovedBy"),
                }
            };
        }       
    }
}
