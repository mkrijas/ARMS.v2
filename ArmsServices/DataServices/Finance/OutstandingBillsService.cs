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
        IEnumerable<OutstandingBillsModel> SelectByParty(int? PartyID, int? PartyBranchID,int? BranchID);
        IEnumerable<OutstandingBillsModel> SelectByPeriod(DateTime? begin, DateTime? end);        
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

        public IEnumerable<OutstandingBillsModel> SelectByParty(int? PartyID, int? PartyBranchID,int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@PartyBranchID", PartyBranchID),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

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

        private OutstandingBillsModel  GetModel(IDataRecord dr)
        {
            return new OutstandingBillsModel
            {
                BillTransactionID = dr.GetInt32("BillTransactionID"),
                BillTransactionType = dr.GetString("BillTransactionType"),
                BoID = dr.GetInt32("BoID"),
                InitialAmount = dr.GetDecimal("InitialAmount"),
                OutstandingAmount = dr.GetDecimal("OutstandingAmount"),
                DocDate = dr.GetDateTime("DocumentDate"),
                DocNumber = dr.GetString("DocumentNumber"),
                BranchName = dr.GetString("BranchName"),
                 
                InvoiceDate = dr.GetDateTime("InvoiceDate"),                
                InvoiceNumber = dr.GetString("InvoiceNumber"),               
                PartyBranchInfo = new PartyBranchModel()
                {
                    GstID = dr.GetInt32("PartyBranchID"),
                    GstNo = dr.GetString("GstNo"),
                    Party = new PartyModel()
                    {
                        PartyID = dr.GetInt32("PartyID"),
                        PartyName = dr.GetString("PartyName")
                    },
                }
            };
        }
    }
}