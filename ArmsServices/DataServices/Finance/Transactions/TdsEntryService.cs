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

        public TdsTransactionModel Update(TdsTransactionModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@IsPayable", model.IsTdsPayable),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),               
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Entries", model.Tds.ToDataTable()),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@PartyID", model.Party.PartyID),
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

        private TdsTransactionModel GetModel(IDataRecord dr)
        {
            return new TdsTransactionModel
            {
                ID = dr.GetInt32("ID"),
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
