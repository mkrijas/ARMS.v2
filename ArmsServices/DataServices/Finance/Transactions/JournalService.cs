


using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;



namespace ArmsServices.DataServices
{
    public interface IJournalService
    {
        JournalModel Update(JournalModel model);
        JournalModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<JournalModel> Select(int? BranchID);
        IEnumerable<JournalModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<JournalModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm);
        IEnumerable<JournalModel> SelectByPeriod(DateTime? begin, DateTime? end);       
        int Approve(int? ID, string UserID, string Remarks);
        int Reverse(int? ID, string UserID, string Remarks);
    }


    public class JournalService : IJournalService
    {
        IDbService Iservice;       
        public JournalService(IDbService iservice)
        {
            Iservice = iservice;          
        }

        public int Approve(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JournalID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
               //new SqlParameter("@Status", 1)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Journal.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Journal.Delete]", parameters);
        }

     
        public int Reverse(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@UserID", UserID)

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Journal.Reverse]", parameters);
        }

        public IEnumerable<JournalModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Journal.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<JournalModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "ByApproved"),
                new SqlParameter("@BranchID", BranchID),
                new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Journal.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<JournalModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "ByUnapproved"),
                new SqlParameter("@BranchID", BranchID),
                new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Journal.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }




        public JournalModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            JournalModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Journal.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

       
        public IEnumerable<JournalModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Journal.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public JournalModel Update(JournalModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JournalID", model.JournalID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@DebitCoaID", model.Debit.CoaID),
               new SqlParameter("@CreditCoaID", model.Credit.CoaID),
               new SqlParameter("@Reference", model.Reference),             
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@MID", model.MID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),               
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),              
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Journal.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private JournalModel GetModel(IDataRecord dr)
        {
            return new JournalModel
            {
                JournalID = dr.GetInt32("JournalID"),
                
                Reference = dr.GetString("Reference"),
                Debit = new ChartOfAccountModel() { CoaID = dr.GetInt32("DebitCoaID"),AccountName =dr.GetString("Debit")},
                Credit = new ChartOfAccountModel() { CoaID = dr.GetInt32("CreditCoaID"), AccountName = dr.GetString("Credit") },
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                CostCenter = dr.GetInt32("CostCenter"),

                Dimension = dr.GetInt32("Dimension"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
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

