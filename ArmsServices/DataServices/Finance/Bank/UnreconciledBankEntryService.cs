
using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;
using System;

namespace ArmsServices.DataServices
{
    public class UnreconciledBankEntryService : IUnreconciledBankEntryService
    {
        IDbService Iservice;

        // Constructor to initialize the database service
        public UnreconciledBankEntryService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Deletes an unreconciled bank entry by ID and UserID
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.BankAccount.UnreconciledEntry.Delete]", parameters);
        }

        // Reconciles a bank entry
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

        // Retrieves reconciliation info for a given UnreconciledEntryID
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

        // Retrieves unreconciled bank entries based on BankID
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

        // Retrieves unreconciled bank entries based on BranchID
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

        // Retrieves all reconciled bank entries based on BranchID and BankID
        public IEnumerable<ReconciledBankEntryModel> SelectAllUnReconciledBank(int? BranchID, int? BankID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID",BranchID),
               new SqlParameter("@BankID",BankID),
               new SqlParameter("@Operation","GetAllUnreconciledEntry" ),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.UnreconciledEntry.Select]", parameters))
            {
                yield return GetReconciledModel(dr);
            }
        }

        // Maps data from IDataRecord to UnReconciledBankEntryModel
        public IEnumerable<ReconciledBankEntryModel> SelectAllReconciledBank(int? BranchID, int? BankID, DateTime? StartDate, DateTime? EndDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID",BranchID),
               new SqlParameter("@BankID",BankID),
               new SqlParameter("@StartDate",StartDate?.ToString("yyyy/MM/dd")??null),
               new SqlParameter("@EndDate",EndDate?.ToString("yyyy/MM/dd")?? null),
               new SqlParameter("@Operation","GetAllReconciledEntry" ),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.UnreconciledEntry.Select]", parameters))
            {
                yield return GetReconciledModel(dr);
            }
        }

        // Retrieves a list of bank summary records for both "Company" and "Bank" using the provided BranchID, ArdCode, StartDate, and EndDate filters.     
        public List<ReconciledBankSummaryModel> GetReconcilBankSummary(int? BranchID, string ArdCode, DateTime? StartDate, DateTime? EndDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID",BranchID),
               new SqlParameter("@ArdCode",ArdCode),
               new SqlParameter("@StartDate",StartDate?.ToString("yyyy/MM/dd")??null),
               new SqlParameter("@EndDate",EndDate?.ToString("yyyy/MM/dd")?? null),
            };
            List<ReconciledBankSummaryModel> model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.UnreconciledEntry.Summary.Select]", parameters))
            {
                model.Add(new ReconciledBankSummaryModel()
                {
                    BankOrCompany = "Company",
                    OpeningAmount = dr.GetDecimal("CompanyOpeningAmount") ?? 0,
                    TransactionAmount = dr.GetDecimal("CompanyTransactionAmount") ?? 0,
                    ClossingAmount = dr.GetDecimal("CompanyClossingAmount") ?? 0
                });

                model.Add(new ReconciledBankSummaryModel()
                {
                    BankOrCompany = "Bank",
                    OpeningAmount = dr.GetDecimal("BankOpeningAmount"),
                    TransactionAmount = dr.GetDecimal("BankTransactionAmount"),
                    ClossingAmount = dr.GetDecimal("BankClossingAmount")
                });
            }
            return model;
        }

        // Retrieves a single unreconciled bank entry based on its ID.
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

        // Updates reconciled bank entry details using a list of ReconcileUpdateModel and returns the updated ReconciledBankEntryModel.
        public ReconciledBankEntryModel UpdateUnReconciledBankEntry(List<ReconcileUpdateModel> lst,string userID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@lst",lst.ToDataTable()),
                new SqlParameter("@userID",userID),
               //new SqlParameter("@ID",reconciledBankEntry.ID),
               //new SqlParameter("@BankID",reconciledBankEntry.BankID),
               //new SqlParameter("@IsExisting",reconciledBankEntry.IsExisting),
               //new SqlParameter("@ReconciledDate",reconciledBankEntry.ReconciledDate),
               //new SqlParameter("@AccountEntryID",reconciledBankEntry.AccountEntryID),
               //new SqlParameter("@Remarks",reconciledBankEntry.Remarks),
               //new SqlParameter("@UserID",reconciledBankEntry.UserInfo.UserID),
            };
            ReconciledBankEntryModel reconciledBankModel = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.ReconciledEntry.Update]", parameters))
            {
                reconciledBankModel = GetReconciledModel(dr);
            }
            return reconciledBankModel;
        }

        // Updates an unreconciled bank entry record with the provided details and returns the updated model.
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
               //new SqlParameter("@ReconciledDate", model.IsReconciled),
               new SqlParameter("@TransactionDate", model.TransactionDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@RecordStatus", 3),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BankAccount.UnReconciledBankEntry.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Updates an unreconciled bank entry record with the provided details and returns the updated model.
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
                PaymentRemarks = dr.GetString("PaymentRemarks"),
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

        // Maps a data record to a ReconciledBankEntryModel, including its associated user information
        private ReconciledBankEntryModel GetReconciledModel(IDataRecord dr)
        {
            return new ReconciledBankEntryModel()
            {
                Amount = dr.GetDecimal("Amount"),
                BankID = dr.GetInt32("BankID"),
                IsExisting = dr.GetBoolean("IsExisting"),
                ReconciledDate = dr.GetDateTime("ReconciledDate"),
                AccountEntryID = dr.GetInt64("AccountEntryID"),
                AccountName = dr.GetString("AccountName"),
                DocDate = dr.GetDateTime("DocDate"),
                Remarks = dr.GetString("Remarks"),
                DocNumber = dr.GetString("DocNumber"),
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