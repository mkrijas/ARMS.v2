
using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;

namespace ArmsServices.DataServices.Finance.Bank
{

    public interface IBankAccountOwnService
    {
        UnReconciledBankEntryModel Update(UnReconciledBankEntryModel model);
        int Delete(int? ID, string UserID);
        IEnumerable<UnReconciledBankEntryModel> Select(int? BankID, bool ShowOnlyUnreconciled);
        IEnumerable<UnReconciledBankEntryModel> SelectByBranch(int? BranchID);        
        ReconciledBankEntryModel GetRecociledInfo(int? UnreconciledEntryID);
        UnReconciledBankEntryModel SelectByID(int? ID);
        ReconciledBankEntryModel Reconcile(ReconciledBankEntryModel model);
    }

    public class UnreconciledBankEntryService : IBankAccountOwnService
    {
        IDbService Iservice;        
        public UnreconciledBankEntryService(IDbService iservice)
        {
            Iservice = iservice;           
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.BankAccount.UnreconciledEntry.Delete]", parameters);
        }

        public ReconciledBankEntryModel Reconcile(ReconciledBankEntryModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UnreconciledEntryID", model.ID),               
               new SqlParameter("@ReconciledDate", model.ReconciledDate),
               new SqlParameter("@ReconciledDate", model.Remarks),
               new SqlParameter("@UserID", model.UserInfo.UserID),

            };
            foreach (var dr in Iservice.GetDataReader("[usp.Finance.BankAccount.ReconciledEntry.Update]", parameters))
            {
                new ReconciledBankEntryModel()
                {

                    ID = dr.GetInt32("RecID"),
                    ReconciledDate = dr.GetDateTime("ReconciledDate"),
                    Remarks = dr.GetString("Remarks"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecRecordStatus"),
                        TimeStampField = dr.GetDateTime("RecTimeStamp"),
                        UserID = dr.GetString("RecUserID"),
                    },
                };
            }
            return null;
        }

        public ReconciledBankEntryModel GetRecociledInfo(int? UnreconciledEntryID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UnreconciledEntryID",UnreconciledEntryID),
               new SqlParameter("@Operation","ByURID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.ReconciledEntry.Select]", parameters))
            {
                return new ReconciledBankEntryModel()
                {
                    ID = dr.GetInt32("ID"),
                    ReconciledDate = dr.GetDateTime("ReconciledDate"),
                    Remarks = dr.GetString("Remarks"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
            return null;
        }

        public IEnumerable<UnReconciledBankEntryModel> Select(int? BankID, bool ShowOnlyUnreconciled)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ShowOnlyUnreconciled",ShowOnlyUnreconciled),
               new SqlParameter("@BankID",BankID),
               new SqlParameter("@Operation","ByBankID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.UnreconciledEntry.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<UnReconciledBankEntryModel> SelectByBranch(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID",BranchID),              
               new SqlParameter("@Operation","ByBranchID" ),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.UnreconciledEntry.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public UnReconciledBankEntryModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID",ID),              
               new SqlParameter("@Operation","ByID"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.UnreconciledEntry.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        public UnReconciledBankEntryModel Update(UnReconciledBankEntryModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@ArdCode", model.ArdCode),
               new SqlParameter("@InstrumentDate", model.InstrumentDate),
               new SqlParameter("@BankID", model.BankID),
               new SqlParameter("@InstrumentReference", model.InstrumentReference),
               new SqlParameter("@InstrumentType", model.InstrumentType),
               new SqlParameter("@Nature", model.Nature),
               new SqlParameter("@PaymentRemarks", model.PaymentRemarks),
               new SqlParameter("@ReconciledDate", model.IsReconciled),
               new SqlParameter("@TransactionDate", model.TransactionDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.UnReconciledBankEntry.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        private UnReconciledBankEntryModel GetModel(IDataRecord dr)
        {
            return new UnReconciledBankEntryModel()
            {
                ID = dr.GetInt32("ID"),
                Amount = dr.GetDecimal("Amount"),
                ArdCode = dr.GetString("ArdCode"),
                InstrumentDate = dr.GetDateTime("InstrumentDate"),
                InstrumentReference = dr.GetString("InstrumentReference"),
                InstrumentType = dr.GetString("InstrumentType"),
                Nature = dr.GetInt32("Nature"),
                PaymentRemarks =  dr.GetString("PaymentRemarks"),
                IsReconciled = dr.GetBoolean("IsReconciled"),
                TransactionDate = dr.GetDateTime("TransactionDate"),
                ReconciledInfo = new ReconciledBankEntryModel()
                {

                    ID = dr.GetInt32("RecID"),
                    ReconciledDate = dr.GetDateTime("ReconciledDate"),
                    Remarks = dr.GetString("Remarks"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecRecordStatus"),
                        TimeStampField = dr.GetDateTime("RecTimeStamp"),
                        UserID = dr.GetString("RecUserID"),
                    },
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
