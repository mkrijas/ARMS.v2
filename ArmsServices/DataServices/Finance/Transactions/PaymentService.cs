using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class PaymentService : IPaymentService
    {
        IDbService Iservice;

        public PaymentService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int? PID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentMemoID", PID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.PaymentMemo.Approve]", parameters);
        }


        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentMemoID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.PaymentMemo.Delete]", parameters);
        }

        public IEnumerable<BillsPaidModel> GetBills(int? PID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetBills"),
               new SqlParameter("@PaymentMemoID", PID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return new BillsPaidModel()
                {
                    MID = dr.GetInt32("MID"),
                    BpID = dr.GetInt32("BpID"),
                    BranchID = dr.GetInt32("BranchID"),
                    BranchName = dr.GetString("BranchName"),
                    InvoiceDate = dr.GetDateTime("ReferenceDocDate"),
                    InvoiceNumber = dr.GetString("ReferenceDocNo"),
                    PayAmount = dr.GetDecimal("PayAmount"),
                };
            }
        }




        public int Reverse(int? PID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentMemoID", PID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.PaymentMemo.Approve]", parameters);
        }

        public IEnumerable<PaymentMemoModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<PaymentMemoModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<PaymentMemoModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public IEnumerable<PaymentMemoModel> SelectInterBranch(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", true),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<PaymentMemoModel> SelectInterBranchByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", true),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<PaymentMemoModel> SelectInterBranchByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsInterBranch", true),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<PaymentMemoModel> SelectInitiated(int? PaymentInitiatedID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPIID"),               
               new SqlParameter("@PIID", PaymentInitiatedID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public PaymentMemoModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentMemoID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            PaymentMemoModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<PaymentMemoModel> SelectByParty(int? PartyID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<PaymentMemoModel> SelectByPeriod(DateTime? begin, DateTime? end, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public PaymentMemoModel Update(PaymentMemoModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentMemoID", model.PaymentMemoID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@PaymentStatus", model.PaymentStatus),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@BusinessNature", model.BusinessNature),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@Bills", model.Bills.ToDataTable()),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@PartyID", model.PartyInfo.PartyID),
               new SqlParameter("@PartyCode", model.PartyInfo.PartyCode),
               new SqlParameter("@PartyCoaID", model.PartyCoaID),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@IsInterBranch", model.IsInterBranch),
               new SqlParameter("@InterBranchTranID", model.InterBranchTranID),
               new SqlParameter("@OtherBranchID", model.OtherBranchID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private PaymentMemoModel GetModel(IDataRecord dr)
        {
            return new PaymentMemoModel
            {
                PaymentMemoID = dr.GetInt32("PaymentMemoID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                PaymentStatus = dr.GetByte("PaymentStatus"),
                BusinessNature = dr.GetString("BusinessNature"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                FileName = dr.GetString("FilePath"),
                CostCenter = dr.GetInt32("CostCenter"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                Dimension = dr.GetInt32("Dimension"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                InterBranchTranID = dr.GetInt32("InterBranchTranID"),
                IsInterBranch = dr.GetBoolean("IsInterBranch"),
                OtherBranchID = dr.GetInt32("OtherBranchID"),
                OtherBranchName = dr.GetString("OtherBranchName"),
                PartyCoaID = dr.GetInt32("PartyCoaID"),
                PartyInfo = new PartyModel()
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

        public IEnumerable<PaymentMemoModel> SelectPending(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPending"),
               new SqlParameter("@BranchID", BranchID),               
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
    }
}
