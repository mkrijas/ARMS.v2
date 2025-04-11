using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;



namespace ArmsServices.DataServices
{
    public class JournalService : IJournalService
    {
        IDbService Iservice;
        public JournalService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to approve a journal entry
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

        // Method to delete a journal entry
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Journal.Delete]", parameters);
        }

        // Method to reverse a journal entry
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

        // Method to select journal entries based on Branch ID
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

        // Method to select approved journal entries
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

        // Method to select unapproved journal entries
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

        // Method to select a journal entry by its ID
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

        // Method to select journal entries by date period
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

        // Method to retrieve a list of sub-items associated with a specific journal entry
        public IEnumerable<JournalSubModel> GetSubList(int? JournalID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetSubList"),
               new SqlParameter("@JournalID", JournalID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Journal.Select]", parameters))
            {
                yield return new JournalSubModel()
                {
                    JournalSubID = dr.GetInt32("JournalSubID"),
                    JournalID = dr.GetInt32("JournalID"),
                    Reference = dr.GetString("Reference"),
                    CoaID = dr.GetInt32("CoaID"),
                    Coa = new ChartOfAccountModel() 
                    { 
                        CoaID = dr.GetInt32("CoaID"), 
                        AccountName = dr.GetString("AccountName") 
                    },
                    DrCrType = dr.GetInt32("DrCrType"),
                    Amount = dr.GetDecimal("Amount"),
                    CostCenterVal = dr.GetString("CostCenter"),
                    DimensionVal = dr.GetString("Dimension"),
                    CostCenter = dr.GetInt32("CostCenterID"),
                    Dimension = dr.GetInt32("DimensionID")
                };
            }
        }

        // Method to update JournalModel record
        public JournalModel Update(JournalModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@JournalID", model.JournalID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@MID", model.MID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),               
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@JournalSub", model.JournalSubList.ToDataTable()),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Journal.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Private method to convert an IDataRecord to a JournalModel
        private JournalModel GetModel(IDataRecord dr)
        {
            return new JournalModel
            {
                JournalID = dr.GetInt32("JournalID"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),                
                FileName= dr.GetString("FilePath"),                
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

        public IEnumerable<JournalModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JournalModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public int RemoveFile(int? ID, string UserID)
        {
            throw new NotImplementedException();
        }
    }
}

