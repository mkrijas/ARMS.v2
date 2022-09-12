using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IReceiptService
    {
        ReceiptModel Update(ReceiptModel model);
        ReceiptModel SelectByID(int? ReceiptID);
        int Delete(int? ID, string UserID);
        IEnumerable<ReceiptModel> Select(int? BranchID);
        IEnumerable<ReceiptModel> SelectByParty(int? PartyID, int? PartyBranchID, int? BranchID);
        IEnumerable<ReceiptModel> SelectByPeriod(DateTime? begin, DateTime? end, int? BranchID);        
        IEnumerable<BillsReceiptModel> GetBills(int? ReceiptID);
        int Approve(int? PID, string UserID);
        int Reverse(int? PID, string UserID);        
        //IEnumerable<PaymentEntryModel> GetPaymentEntries(int? PfID);
    }

    public class ReceiptService : IReceiptService
    {
        IDbService Iservice;
        public ReceiptService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public int Approve(int? PID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ReceiptID", PID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Status", 1)
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
                    BoID = dr.GetInt32("BoID"),
                    BrID = dr.GetInt32("BpID"),
                    BranchID = dr.GetInt32("BranchID"),
                    BranchName = dr.GetString("BranchName"),
                    DocDate = dr.GetDateTime("DocumentDate"),
                    DocNumber = dr.GetString("DocumentNumber"),
                    InvoiceDate = dr.GetDateTime("InvoiceDate"),
                    InvoiceNumber = dr.GetString("InvoiceNumber"),
                    ReceiptAmount = dr.GetDecimal("PayAmount"),
                };
            }
        }      

        public int Reverse(int? PID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ReceiptID", PID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Status", 2)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Receipt.Approve]", parameters);
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
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@Bills", model.Bills.ToDataTable()),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@PartyBranchID", model.PartyBranchInfo.GstID),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
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
                ReceiptType = dr.GetString("ReceiptType"),
                BranchID = dr.GetInt32("BranchID"),                
                ApprovedInfo = new ArmsModels.SharedModels.UserInfoModel()
                {
                    RecordStatus = dr.GetByte("ApprovedStatus"),
                    TimeStampField = dr.GetDateTime("ApprovedOn"),
                    UserID = dr.GetString("ApprovedBy"),
                },
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                ReceiptMode = dr.GetString("ReceiptMode"),
                ReceiptTool = dr.GetString("ReceiptTool"),
                ReceiptCoa = dr.GetInt32("ReceiptCoa"),
                IsRealized = dr.GetBoolean("IsRealized"),
                Reference = dr.GetString("Reference"),
                EffectiveDate = dr.GetDateTime("EffectiveDate"),
                CostCenter = dr.GetInt32("CostCenter"),
                Dimension = dr.GetInt32("Dimension"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                PartyBranchInfo = new PartyBranchModel()
                {
                    GstID = dr.GetInt32("PartyBranchID"),
                    Party = new PartyModel()
                    {
                        PartyID = dr.GetInt32("PartyID"),
                        PartyName = dr.GetString("PartyName")
                    },                    
                },
                PartyBranchCoa = dr.GetInt32("PartyBranchCoa"),
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
