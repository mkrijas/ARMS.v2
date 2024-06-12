using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Security.Cryptography;


namespace ArmsServices.DataServices
{
    public class OutstandingBillsService : IOutstandingBillsService
    {
        IDbService Iservice;

        public OutstandingBillsService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AutoSettleID", ID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.OutstandingBills.AutoSettle.Approve]", parameters);
        }

        public int? AutoSettle(AutoSettleModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "AutoSettle"),
               new SqlParameter("@AutoSettleID", model.AutoSettleID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@PartyID", model.PartyInfo.PartyID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@Bills", model.Bills.ToDataTable()),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.OutstandingBills.AutoSettle.Update]", parameters);
        }

        public int DeleteAutoSettle(int? ID, string userID)
        {
            //throw new NotImplementedException();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "DELETE"),
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", userID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.OutstandingBills.AutoSettle.Delete]", parameters);
        }

        public IEnumerable<BillsPaidModel> GetAutoSettledBills(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetBills"),
               new SqlParameter("@AutoSettleID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.AutoSettle.Select]", parameters))
            {
                yield return new BillsPaidModel()
                {                    
                    MID = dr.GetInt32("MID"),
                    BpID = dr.GetInt32("BpID"),
                    BranchID = dr.GetInt32("BranchID"),
                    BranchName = dr.GetString("BranchName"),
                    InvoiceDate = dr.GetDateTime("ReferenceDocDate"),
                    InvoiceNumber = dr.GetString("ReferenceDocNo"),
                    PayAmount = dr.GetDecimal("Amount"),
                    CoaID = dr.GetInt32("CoaID"),
                    IsMemo = dr.GetBoolean("IsMemo"),
                    OutstandingAmount = dr.GetDecimal("Amount"),
                };
            }
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

        public IEnumerable<AutoSettleModel> SelectAutoSettledEntriesByApproved(int? BranchID, int numberOfRecords)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", numberOfRecords),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.AutoSettle.Select]", parameters))
            {
                yield return new AutoSettleModel()
                {
                    PartyInfo = new PartyModel()
                    {
                        PartyID = dr.GetInt32("PartyID"),
                        TradeName = dr.GetString("tradeName"),
                    },
                    AutoSettleID = dr.GetInt32("AutoSettleID"),
                    BranchID = dr.GetInt32("BranchID"),
                    DocumentDate = dr.GetDateTime("DocumentDate"),
                    DocumentNumber = dr.GetString("DocumentNumber"),
                    NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                    MID = dr.GetInt32("MID"),
                    Narration = dr.GetString("Narration"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
                    AuthLevelId = dr.GetInt32("AuthLevelId"),
                    AuthStatus = dr.GetString("AuthStatus")
                };
            }
        }

        public IEnumerable<AutoSettleModel> SelectAutoSettledEntriesByUnapproved(int? BranchID, int numberOfRecords)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", numberOfRecords),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.AutoSettle.Select]", parameters))
            {
                yield return new AutoSettleModel()
                {
                    PartyInfo = new PartyModel()
                    {
                        PartyID = dr.GetInt32("PartyID"),
                        TradeName = dr.GetString("tradeName"),
                    },
                    AutoSettleID = dr.GetInt32("AutoSettleID"),
                    BranchID = dr.GetInt32("BranchID"),
                    DocumentDate = dr.GetDateTime("DocumentDate"),
                    DocumentNumber = dr.GetString("DocumentNumber"),
                    NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                    MID = dr.GetInt32("MID"),
                    Narration = dr.GetString("Narration"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
                    AuthLevelId = dr.GetInt32("AuthLevelId"),
                    AuthStatus = dr.GetString("AuthStatus")
                };
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

        //public IEnumerable<OutstandingBillsModel> SelectByParty(int? PartyID, int? BranchID, int? PartyBranchID)
        //{
        //    throw new NotImplementedException();
        //}

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

        //public IEnumerable<OutstandingBillsModel> SelectOutstandingPayments(int? PartyID, int? BranchID)
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>
        //    {
        //       new SqlParameter("@Operation", "ByBranch"),
        //       new SqlParameter("@BranchID", BranchID),
        //       new SqlParameter("@PartyID", PartyID),
        //    };
        //    foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingPayments.Select]", parameters))
        //    {
        //        yield return new OutstandingBillsModel()
        //        {
        //            ReferenceDocDate = dr.GetDateTime("ReferenceDocDate"),
        //            ReferenceDocNo = dr.GetString("ReferenceDocNo"),
        //            OutstandingAmount = dr.GetDecimal("OutstandingAmount"),                     
        //             BranchName = dr.GetString("BranchName"),
        //             PartyInfo = new PartyModel()
        //             {
        //                 PartyID = dr.GetInt32("PartyID"),
        //                 PartyCode= dr.GetString("PartyID"),
        //             },
        //             //OpID = dr.GetInt32("OpID"),
        //             //PaymentTransactionID = dr.GetInt32("PaymentTransactionID"),
        //             //PaymentTransactionType = dr.GetString("PaymentTransactionType"),
        //             //ReferenceDate = dr.GetDateTime("ReferenceDate"),
        //             //ReferenceNumber = dr.GetString("ReferenceNumber"),
        //        };
        //    }
        //}

        //public int SettleBillsToPayment(int? OPID, List<BillsReceiptModel> Bills)
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>
        //    {
        //       new SqlParameter("@Operation", "Settle"),
        //       new SqlParameter("@Bills", Bills.ToDataTable()),
        //       new SqlParameter("@OPID", OPID),
        //    };
        //    return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.OutstandingPayments.SettleBills]", parameters);
            
        //}

        private OutstandingBillsModel  GetModel(IDataRecord dr)
        {
            return new OutstandingBillsModel
            {                
                BoID = dr.GetInt32("BoID"),
                MID = dr.GetInt32("MID"),
                OutstandingAmount = dr.GetDecimal("OutstandingAmount"),
                NatureOfTransaction= dr.GetString("NatureOfTransaction"),                
                BranchName = dr.GetString("BranchName"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentNumber= dr.GetString("DocNumber"),
                DocumentDate = dr.GetDateTime("DocDate"),                
                ReferenceDocDate = dr.GetDateTime("ReferenceDocDate"),
                ReferenceDocNo = dr.GetString("ReferenceDocNo"),
                isMemo=dr.GetBoolean("IsMemo"),
                CoaID = dr.GetInt32("CoaID"),  
                SubArdCode = dr.GetString("SubArdCode"),
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