using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class ContraService : IContraService
    {
        IDbService Iservice;


        public ContraService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ContraID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Contra.Delete]", parameters);

        }

        public IEnumerable<ContraModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
                new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Contra.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<ContraModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords), 
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Contra.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public IEnumerable<ContraModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
                new SqlParameter("@BranchID", BranchID),
                new SqlParameter("@numberOfRecords", NumberOfRecords),
                new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Contra.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public IEnumerable<ContraModel> SelectInterBranch(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@InterBranch", true),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Contra.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<ContraModel> SelectInterBranchByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@InterBranch", true),
               new SqlParameter("@numberOfRecords", NumberOfRecords), 
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Contra.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<ContraModel> SelectInterBranchByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@InterBranch", true),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Contra.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ContraModel SelectByID(int? ID)
        {
            throw new NotImplementedException();
        }
        public int Approve(int? ID, string UserID,string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ContraID", ID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@UserID", UserID),
               
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Contra.Approve]", parameters);
        }

        public int Reverse(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ContraID", ID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Contra.Reverse]", parameters);
        }
        public ContraModel Update(ContraModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ContraID", model.ContraID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@ContraModeHome", model.ContraModeHome),
               new SqlParameter("@ArdCodeHome", model.ArdCodeHome),
               new SqlParameter("@CoaIDHome", model.CoaIDHome),
               new SqlParameter("@BranchIDOther", model.OtherBranchID),
               new SqlParameter("@ContraModeOther", model.ContraModeOther),
               new SqlParameter("@ArdCodeOther", model.ArdCodeOther),
               new SqlParameter("@CoaIDOther", model.CoaIDOther),               
               new SqlParameter("@Reference", model.EntryReference),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@PaymentTool", model.PaymentTool),
               new SqlParameter("@BankCharges", model.BankCharges),
               new SqlParameter("@IsPayment", model.IsPayment),              
               new SqlParameter("@ChequeNumber", model.ChequeInfo.ChequeNumber),
               new SqlParameter("@ChequeDate", model.ChequeInfo.ChequeDate),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@IsInterBranch", model.IsInterBranch),
               new SqlParameter("@InterBranchTranID", model.InterBranchTranID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Contra.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        private ContraModel GetModel(IDataRecord dr)
        {
            return new ContraModel
            {              
                ContraID = dr.GetInt32("ContraID"),
                ContraModeHome = dr.GetString("ContraModeHome"),
                CoaIDHome = dr.GetInt32("CoaIDHome"),
                ArdCodeHome = dr.GetString("ArdCodeHome"),
                OtherBranchID = dr.GetInt32("BranchIDOther"),
                ContraModeOther = dr.GetString("ContraModeOther"),
                CoaIDOther = dr.GetInt32("CoaIDOther"),
                ArdCodeOther = dr.GetString("ArdCodeOther"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                EntryReference = dr.GetString("Reference"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                BranchID = dr.GetInt32("BranchID"),
                IsPayment = dr.GetBoolean("IsPayment"),
                PaymentTool = dr.GetString("PaymentTool"),
                BankCharges = dr.GetDecimal("BankCharges"),
                FileName = dr.GetString("FilePath"),
                InterBranchTranID = dr.GetInt32("InterBranchTranID"),
                IsInterBranch = dr.GetBoolean("IsInterBranch"),
                ChequeInfo = new ChequeModel()
                {
                    ChequeDate = dr.GetDateTime("ChequeDate"),
                    ChequeNumber = dr.GetString("ChequeNumber"),
                },               
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocNumber"),
                MID = dr.GetInt32("MID"),
                CostCenter = dr.GetInt32("CostCenter"),

                Dimension = dr.GetInt32("Dimension"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
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
