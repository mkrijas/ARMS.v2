using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IPaymentService
    {
        PartyPaymentMemoModel Update(PartyPaymentMemoModel model);
        PartyPaymentMemoModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<PartyPaymentMemoModel> Select(int? BranchID);
        IEnumerable<PartyPaymentMemoModel> SelectByParty(int? PartyID, int? PartyBranchID, int? BranchID);
        IEnumerable<PartyPaymentMemoModel> SelectByPeriod(DateTime? begin, DateTime? end,int? BranchID);
        IEnumerable<PartyPaymentMemoModel> Select(int PaymentInitiatedID, int? BranchID);
        IEnumerable<BillsPaidModel> GetBills(int? PID);
        int Approve(int? PID, string UserID,string Remarks);
        int Reverse(int? PID, string UserID, string Remarks);
        int? InitiatePayment(PaymentInitiatedModel model);
        IEnumerable<PaymentInitiatedModel> SelectInitiated(int? BranchID);
        IEnumerable<PaymentFinishModel> SelectFinished(int? BranchID);
        IEnumerable<PaymentInitiatedModel> SelectInitiatedBetween(int? BranchID,DateTime Begin,DateTime End);
        IEnumerable<PaymentFinishModel> SelectFinishedBetween(int? BranchID,DateTime Begin,DateTime End);
        int? CompletePayment(PaymentFinishModel model);
        IEnumerable<PaymentInitiatedModel> PendingForCompletion(int? BranchID);
        IEnumerable<PaymentEntryModel> GetPaymentEntries(int? PfID);
    }

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
               new SqlParameter("@Status", 1)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.PaymentMemo.Approve]", parameters);
        }

        public int? CompletePayment(PaymentFinishModel model)
        {
            
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),              
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@CoaID", model.CoaID),
               new SqlParameter("@PaymentArdCode", model.PaymentArdCode),
               new SqlParameter("@PaymentMode", model.PaymentMode),
               new SqlParameter("@PaymentTool", model.PaymentTool),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@PiID", model.PiID),
               new SqlParameter("@PfID", model.PfID),                         
               new SqlParameter("@Entries", model.Payments.ToDataTable()),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            }; 
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Finish]", parameters))
            {
                model.PfID = dr.GetInt32("PcID");
            }
            return model.PfID;
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
                    BoID = dr.GetInt32("BoID"),
                    BpID = dr.GetInt32("BpID"),
                    BranchID = dr.GetInt32("BranchID"),
                    BranchName = dr.GetString("BranchName"),
                    DocDate = dr.GetDateTime("DocumentDate"),
                    DocNumber=dr.GetString("DocumentNumber"),
                    InvoiceDate=dr.GetDateTime("InvoiceDate"),
                    InvoiceNumber=dr.GetString("InvoiceNumber"),

                    
                    PayAmount = dr.GetDecimal("PayAmount"),
                };
            }
        }

        public IEnumerable<PaymentEntryModel> GetPaymentEntries(int? PfID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetPayEntries"),
               new SqlParameter("@PfID", PfID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Finish.Select]", parameters))
            {
                yield return new PaymentEntryModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    EffectiveDate = dr.GetDateTime("EffectiveDate"),
                    PaymentMemoID = dr.GetInt32("PaymentMemoID"),
                    PeID = dr.GetInt32("PeID"),
                    Reference = dr.GetString("Reference"),
                };
            }
        }

        public int? InitiatePayment(PaymentInitiatedModel model)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("memo");

            foreach (var item in model.PaymentMemos)
            {
                DataRow row = dt.NewRow();
                row["memo"] = item.PaymentMemoID;
                 dt.Rows.Add(row);
            }
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PiID", model.PiID),
               new SqlParameter("@DueOn", model.DueOn),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@memos", dt),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Initiate]", parameters))
            {
                model.PiID = dr.GetInt32("PiID");
            }
            return model.PiID;
        }

        public IEnumerable<PaymentInitiatedModel> PendingForCompletion(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ToComplete"),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return new PaymentInitiatedModel()
                {
                     BranchID = dr.GetInt32("BranchID"),
                     DueOn = dr.GetDateTime("DueOn"),
                     DocNumber = dr.GetString("DocNumber"),
                    DocDate = dr.GetDateTime("DocDate"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
                     PiID = dr.GetInt32("PiID"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
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
               new SqlParameter("@Status", 2)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.PaymentMemo.Approve]", parameters);
        }

        public IEnumerable<PartyPaymentMemoModel> Select(int? BranchID)
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

        public IEnumerable<PartyPaymentMemoModel> Select(int PaymentInitiatedID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPIID"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@PiID", PaymentInitiatedID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public PartyPaymentMemoModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentMemoID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            PartyPaymentMemoModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<PartyPaymentMemoModel> SelectByParty(int? PartyID, int? PartyBranchID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@PartyBranchID", PartyBranchID),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<PartyPaymentMemoModel> SelectByPeriod(DateTime? begin, DateTime? end, int? BranchID)
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

        public IEnumerable<PaymentFinishModel> SelectFinished(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Finish.Select]", parameters))
            {
                yield return new PaymentFinishModel()
                {
                    BranchID = dr.GetInt32("BranchID"),
                    PfID = dr.GetInt32("PfID"),
                    PiID = dr.GetInt32("PiID"),
                    CoaID = dr.GetInt32("CoaID"),
                    DocumentDate = dr.GetDateTime("DocumentDate"),
                    Narration = dr.GetString("Narration"),
                    PaymentMode = dr.GetString("PaymentMode"),
                    PaymentTool = dr.GetString("PaymentTool"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
        }

        public IEnumerable<PaymentFinishModel> SelectFinishedBetween(int? BranchID, DateTime Begin, DateTime End)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", Begin),
               new SqlParameter("@end", End),
               new SqlParameter("@BranchID", BranchID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Finish.Select]", parameters))
            {
                yield return new PaymentFinishModel()
                {
                    BranchID = dr.GetInt32("BranchID"),
                    PfID = dr.GetInt32("PfID"),
                    PiID = dr.GetInt32("PiID"),
                    CoaID = dr.GetInt32("CoaID"),
                    DocumentDate = dr.GetDateTime("DocumentDate"),
                    Narration = dr.GetString("Narration"),
                    PaymentMode = dr.GetString("PaymentMode"),
                    PaymentTool = dr.GetString("PaymentTool"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
        }

        public IEnumerable<PaymentInitiatedModel> SelectInitiated(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
            };


            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Initiate.Select]", parameters))
            {
                yield return new PaymentInitiatedModel()
                {
                    BranchID = dr.GetInt32("BranchID"),
                    DueOn = dr.GetDateTime("DueOn"),
                    PiID = dr.GetInt32("PiID"),
                    DocNumber = dr.GetString("DocNumber"),
                    DocDate = dr.GetDateTime("DocDate"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
        }

        public IEnumerable<PaymentInitiatedModel> SelectInitiatedBetween(int? BranchID, DateTime Begin, DateTime End)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", Begin),
               new SqlParameter("@end", End),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Initiate.Select]", parameters))
            {
                yield return new PaymentInitiatedModel()
                {
                    BranchID = dr.GetInt32("BranchID"),
                    DueOn = dr.GetDateTime("DueOn"),
                    PiID = dr.GetInt32("PiID"),
                    DocNumber = dr.GetString("DocNumber"),
                    DocDate = dr.GetDateTime("DocDate"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
        }

        public PartyPaymentMemoModel Update(PartyPaymentMemoModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PaymentMemoID", model.PaymentMemoID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@PaymentInitiatedID", model.PaymentInitiatedID),
               new SqlParameter("@PaymentStatus", model.PaymentStatus),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@Bills", model.Bills.ToDataTable()),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@PartyID", model.PartyInfo.PartyID),
               new SqlParameter("@PartyCode", model.PartyInfo.PartyCode),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.PaymentMemo.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private PartyPaymentMemoModel GetModel(IDataRecord dr)
        {
            return new PartyPaymentMemoModel
            {
                PaymentMemoID = dr.GetInt32("PaymentMemoID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),                
                PaymentInitiatedID = dr.GetInt32("PaymentInitiatedID"),
                PaymentStatus = dr.GetByte("PaymentStatus"),
                BranchID = dr.GetInt32("BranchID"),
               // BranchName = dr.GetString("BranchName"),              
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                CostCenter = dr.GetInt32("CostCenter"),               
                Dimension = dr.GetInt32("Dimension"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
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
    }
}
