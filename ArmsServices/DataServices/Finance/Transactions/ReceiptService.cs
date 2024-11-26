using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class ReceiptService : IReceiptService
    {
        IDbService Iservice;
        public ReceiptService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public int Approve(int? PID, string UserID, string remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ReceiptID", PID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Receipt.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ReceiptID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Receipt.Delete]", parameters);
        }

        public IEnumerable<BillsReceiptModel> GetBills(int? ReceiptID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetBills"),
               new SqlParameter("@ReceiptID", ReceiptID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Receipt.Select]", parameters))
            {
                yield return new BillsReceiptModel()
                {
                    MID = dr.GetInt32("MID"),
                    BrID = dr.GetInt32("BpID"),
                    BranchID = dr.GetInt32("BranchID"),
                    PartyID = dr.GetInt32("PartyID"),
                    BranchName = dr.GetString("BranchName"),
                    InvoiceDate = dr.GetDateTime("ReferenceDocDate"),
                    InvoiceNumber = dr.GetString("ReferenceDocNo"),
                    ReceiptAmount = dr.GetDecimal("ReceiptAmount"),
                    CoaID = dr.GetInt32("CoaID"),                    
                };
            }
        }

        public IEnumerable<ReceiptModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Receipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<ReceiptModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", IsInterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Receipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<ReceiptModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", IsInterBranch),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Receipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }       

        public ReceiptModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ReceiptID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            ReceiptModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Receipt.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ReceiptModel> SelectByParty(int? PartyID, int? PartyBranchID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@PartyBranchID", PartyBranchID),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Receipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<ReceiptModel> SelectByPeriod(DateTime? begin, DateTime? end, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Receipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ReceiptModel Update(ReceiptModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ReceiptID", model.ReceiptID),
               new SqlParameter("@BusinessNature", model.BusinessNature),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@OtherBranch", model.OtherBranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@BankCharges", model.BankCharges),
               new SqlParameter("@ReceiptMode", model.ReceiptMode),
               new SqlParameter("@ReceiptTool", model.ReceiptTool),
               new SqlParameter("@ReceiptArdCode", model.ReceiptArdCode),
               new SqlParameter("@ReceiptCoa", model.ReceiptCoaID),
               new SqlParameter("@Referece", model.Reference),
               new SqlParameter("@IsRealized", model.IsRealized),
               new SqlParameter("@EffectiveDate", model.EffectiveDate),
               new SqlParameter("@RecordStatus", model.UserInfo.RecordStatus),
               new SqlParameter("@Bills", model.Bills.ToDataTable()),
               //new SqlParameter("@CostCenter", model.CostCenter),
               //new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@PartyID", model.PartyInfo.PartyID),
               new SqlParameter("@PartyCode", model.PartyInfo.PartyCode),
               new SqlParameter("@PartyCoaID", model.PartyCoaID),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@TimeStamp", model.UserInfo.TimeStampField),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@IsInterBranch", model.IsInterBranch),
               new SqlParameter("@InterBranchTranID", model.InterBranchTranID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Receipt.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        private ReceiptModel GetModel(IDataRecord dr)
        {
            return new ReceiptModel
            {
                ReceiptID = dr.GetInt32("ReceiptID"),
                BusinessNature = dr.GetString("BusinessNature"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                ReceiptMode = dr.GetString("ReceiptMode"),
                ReceiptTool = dr.GetString("ReceiptTool"),
                ReceiptCoaID = dr.GetInt32("ReceiptCoa"),
                ReceiptArdCode = dr.GetString("ArdCode"),
                IsRealized = dr.GetBoolean("IsRealized"),
                BankCharges = dr.GetDecimal("BankCharges"),
                Reference = dr.GetString("Referece"),
                EffectiveDate = dr.GetDateTime("EffectiveDate"),
                //CostCenter = dr.GetInt32("CostCenter"),
                FileName = dr.GetString("FilePath"),
                //Dimension = dr.GetInt32("Dimension"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                InterBranchTranID = dr.GetInt32("InterBranchTranID"),
                IsInterBranch = dr.GetBoolean("IsInterBranch"),
                OtherBranchID = dr.GetInt32("OtherBranch"),
                PartyInfo = new PartyModel()
                {
                    PartyID = dr.GetInt32("PartyID"),
                    TradeName = dr.GetString("PartyName")
                },
                PartyCoaID = dr.GetInt32("PartyCoa"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }
        public int Reverse(int? PID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ReceiptID", PID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Receipt.Reverse]", parameters);
        }

        public int RemoveFile(int? ID, string UserID)
        {
            throw new NotImplementedException();
        }
    }
}