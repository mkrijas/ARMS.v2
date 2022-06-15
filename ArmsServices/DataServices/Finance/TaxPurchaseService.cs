
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITaxPurchaseService
    {
        TaxPurchaseModel Update(TaxPurchaseModel model);
        TaxPurchaseModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<TaxPurchaseModel> Select();
        IEnumerable<TaxPurchaseModel> SelectByParty(int? PartyID,int? PartyBranchID);
        IEnumerable<TaxPurchaseModel> SelectByPeriod(DateTime? begin,DateTime? end);
        int Approve(int? PID, string UserID);
        int Reverse(int? PID, string UserID);
    }

    public class TaxPurchaseService : ITaxPurchaseService
    {
        IDbService Iservice;

        public TaxPurchaseService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int? PID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", PID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Status", 1)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxPurchase.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", ID),
               new SqlParameter("@UserID", UserID),
              
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxPurchase.Delete]", parameters);
        }

        public int Reverse(int? PID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", PID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Status", 2)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxPurchase.Approve]", parameters);
        }

        public IEnumerable<TaxPurchaseModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }      

        public TaxPurchaseModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            TaxPurchaseModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }      

        public IEnumerable<TaxPurchaseModel> SelectByParty(int? PartyID, int? PartyBranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@PartyBranchID", PartyBranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<TaxPurchaseModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public TaxPurchaseModel Update(TaxPurchaseModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", model.PID),
               new SqlParameter("@AdditionalTDS", model.AdditionalTDS),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@Expenses", model.Expenses.ToDataTable()),
               new SqlParameter("@GRNID", model.GRNID),
               new SqlParameter("@InvoiceDate", model.InvoiceDate),
               new SqlParameter("@InvoiceNo", model.InvoiceNo),
               new SqlParameter("@IsCredit", model.IsCredit),
               new SqlParameter("@CostCenter", model.CostCenter),
                new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@Items", model.Items.ToDataTable()),
               new SqlParameter("@NonStoreInventory", model.NonStoreInventory),
               new SqlParameter("@PartyBranchID", model.PartyBranchInfo.GstID),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private TaxPurchaseModel GetModel(IDataRecord dr)
        {
            return new TaxPurchaseModel
            {
                PID = dr.GetInt32("PID"),
                AdditionalTDS =  dr.GetDecimal("AdditionalTDS"),
                BranchID =  dr.GetInt32("BranchID"),
                ApprovedInfo = new ArmsModels.SharedModels.UserInfoModel() 
                {
                    RecordStatus = dr.GetByte("ApprovedStatus"),
                    TimeStampField = dr.GetDateTime("ApprovedOn"),
                    UserID = dr.GetString("ApprovedBy"),
                },
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                GRNID = dr.GetInt32("GRNID"),
                InvoiceDate =   dr.GetDateTime("InvoiceDate"),
                IsCredit = dr.GetBoolean("IsCredit"),
                InvoiceNo = dr.GetString("InvoiceNo"),
                MID = dr.GetInt32("MID"),
                CostCenter = dr.GetInt32("CostCenter"),
                Dimension = dr.GetInt32("Dimension"),
                NonStoreInventory = dr.GetBoolean("NonStoreInventory"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                PartyBranchInfo = new PartyBranchModel() { GstID = dr.GetInt32("PartyBranchID")},                
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