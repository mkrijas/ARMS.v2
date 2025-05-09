using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class TdsEntryService : ItdsEntryService
    {
        IDbService Iservice;

        public TdsEntryService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to approve a TDS entry
        public int Approve(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TdsReceivable.Approve]", parameters);
        }

        // Method to delete a TDS entryc
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "DELETE"),
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TdsReceivable.Delete]", parameters);
        }

        // Method to get entries associated with a specific TDS transaction
        public IEnumerable<TdsTransactionEntryModel> GetEntries(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetEntries"),
               new SqlParameter("@ID", ID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TdsReceivable.Select]", parameters))
            {
                yield return new TdsTransactionEntryModel()
                {
                    ID = dr.GetInt32("ID"),
                    InvoiceDate = dr.GetDateTime("InvoiceDate"),
                    InvoiceNumber = dr.GetString("InvoiceNumber"),
                    RateOfTds = dr.GetDecimal("RateOfTds"),
                    TaxableAmount = dr.GetDecimal("TaxableAmount"),
                    TdsAmount = dr.GetDecimal("TdsAmount"),
                    TdsNP = dr.GetString("TdsNP"),
                    TdsNpID = dr.GetInt32("TdsNpID"),
                    TransactionID = dr.GetInt32("TrID"),                    
                };
            }
        }

        // Method to remove a file associated with a TDS entry
        public int RemoveFile(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "REMOVEFILE"),
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TdsReceivable.Delete]", parameters);
        }

        public int Reverse(int? ID, string UserID, string Remarks)
        {
            throw new NotImplementedException();
        }

        // Method to select all TDS transactions
        public IEnumerable<TdsTransactionModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TdsReceivable.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<TdsTransactionModel> Select(int? BranchID)
        {
            throw new NotImplementedException();
        }

        // Method to select approved TDS transactions
        public IEnumerable<TdsTransactionModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@InterBranch", InterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TdsReceivable.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a TDS transaction by its ID
        public TdsTransactionModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", ID),
               new SqlParameter("@Operation", "ByID")
            };            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TdsReceivable.Select]", parameters))
            {
               return GetModel(dr);
            }
            return null;
        }

        // Method to select TDS transactions by party
        public IEnumerable<TdsTransactionModel> SelectByParty(int? PartyID, int? PartyBranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@PartyBranchID", PartyBranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TdsReceivable.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select TDS transactions by period
        public IEnumerable<TdsTransactionModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TdsReceivable.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select unapproved TDS transactions
        public IEnumerable<TdsTransactionModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@InterBranch", InterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TdsReceivable.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to update a TDS transaction
        public TdsTransactionModel Update(TdsTransactionModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@TdsType", model.TdsType),
               new SqlParameter("@IsPayable", model.IsTdsPayable),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),               
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Entries", model.Tds.ToDataTable()),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@BusinessNature", model.BusinessNature),
               new SqlParameter("@PartyID", model.Party?.PartyID),
               new SqlParameter("@BankID", model.Bank?.ID),               
               new SqlParameter("@TransactionType", model.TransactionType),
               new SqlParameter("@PartyCoaID", model.PartyCoaID),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TdsReceivable.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to TdsTransactionModel
        private TdsTransactionModel GetModel(IDataRecord dr)
        {
            return new TdsTransactionModel
            {
                ID = dr.GetInt32("ID"),
                TdsType = dr.GetString("TdsType"),
                IsTdsPayable = dr.GetBoolean("IsPayable"),
                PartyCoaID = dr.GetInt32("PartyCoaID"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                MID = dr.GetInt32("MID"),
                FileName = dr.GetString("FilePath"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                Party = new PartyModel()
                {
                    PartyID = dr.GetInt32("PartyID"),
                    TradeName = dr.GetString("TradeName"),
                    PartyCode = dr.GetString("PartyCode"),
                },
                Bank = new OwnBankModel()
                {
                    ID = dr.GetInt32("BankID"),  
                },
                BusinessNature = dr.GetString("BusinessNature"),
                TransactionType = dr.GetString("TransactionType"),
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
