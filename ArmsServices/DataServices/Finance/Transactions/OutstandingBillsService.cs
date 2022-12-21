using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IOutstandingBillsService
    {
        
        OutstandingBillsModel SelectByID(int? ID);       
        IEnumerable<OutstandingBillsModel> Select(int BranchID);
        IEnumerable<OutstandingBillsModel> SelectByParty(int? PartyID, int? BranchID);

       IEnumerable<OutstandingBillsModel> SelectByParty(int? PartyID,int? BranchID, int? PartyBranchID);
        IEnumerable<OutstandingBillsModel> SelectByPeriod(DateTime? begin, DateTime? end);
        int SettleBillsToPayment(int? OPID, List<BillsReceiptModel> Bills);
        IEnumerable<OutstandingPaymentModel> SelectOutstandingPayments(int? PartyID, int? BranchID);
    }

    public class OutstandingBillsService : IOutstandingBillsService
    {
        IDbService Iservice;

        public OutstandingBillsService(IDbService iservice)
        {
            Iservice = iservice;
        }

       
        public IEnumerable<OutstandingBillsModel> Select(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public OutstandingBillsModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BoID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            OutstandingBillsModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<OutstandingBillsModel> SelectByParty(int? PartyID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<OutstandingBillsModel> SelectByParty(int? PartyID, int? BranchID, int? PartyBranchID)
        {
            throw new NotImplementedException();
        }

        //public IEnumerable<OutstandingBillsModel> SelectByParty(int? PartyID, int? PartyBranchID, int? BranchID)
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>
        //    {
        //       new SqlParameter("@PartyBranchID", PartyBranchID),
        //       new SqlParameter("@PartyID", PartyID),
        //       new SqlParameter("@BranchID", BranchID),
        //    };

        //    foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Select]", parameters))
        //    {
        //        yield return GetModel(dr);
        //    }
        //    //   throw new NotImplementedException();
        //}

        public IEnumerable<OutstandingBillsModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<OutstandingPaymentModel> SelectOutstandingPayments(int? PartyID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@PartyID", PartyID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingPayments.Select]", parameters))
            {
                yield return new OutstandingPaymentModel()
                {
                     DocDate = dr.GetDateTime("DocumentDate"),
                     DocNumber = dr.GetString("DocumentNumber"),
                     InitialAmount = dr.GetDecimal("InitialAmount"),                     
                     OutstandingAmount = dr.GetDecimal ("OutstandingAmount"),
                     BranchName = dr.GetString("BranchName"),
                     PartyInfo = new PartyModel()
                     {
                         PartyID = dr.GetInt32("PartyID"),
                     },
                     OpID = dr.GetInt32("OpID"),
                     PaymentTransactionID = dr.GetInt32("PaymentTransactionID"),
                     PaymentTransactionType = dr.GetString("PaymentTransactionType"),
                     ReferenceDate = dr.GetDateTime("ReferenceDate"),
                     ReferenceNumber = dr.GetString("ReferenceNumber"),
                };
            }
        }

        public int SettleBillsToPayment(int? OPID, List<BillsReceiptModel> Bills)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Settle"),
               new SqlParameter("@Bills", Bills.ToDataTable()),
               new SqlParameter("@OPID", OPID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.OutstandingPayments.SettleBills]", parameters);
            
        }

        private OutstandingBillsModel  GetModel(IDataRecord dr)
        {
            return new OutstandingBillsModel
            {                
                BoID = dr.GetInt32("BoID"),
                InitialAmount = dr.GetDecimal("InitialAmount"),
                NatureOfTransaction= dr.GetString("NatureOfTransaction"),                
                BranchName = dr.GetString("BranchName"),
                BranchID= dr.GetInt32("BranchID"),               
                ReferenceDocDate = dr.GetDateTime("ReferenceDocDate"),                
                ReferenceDocNo = dr.GetString("ReferenceDocNo"),               
                PartyInfo = new PartyModel()
                {
                    PartyID = dr.GetInt32("PartyID"),
                    GstNo = dr.GetString("GstNo"),
                    TradeName= dr.GetString("TradeName")
                  
                }
            };
        }
    }
}